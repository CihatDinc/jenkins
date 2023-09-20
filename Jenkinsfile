pipeline {
    agent any
        environment {
            AWS_ACCOUNT_ID      ="212845026981"
            AWS_DEFAULT_REGION  ="eu-central-1" 
            IMAGE_REPO_NAME     ="jenkins-test-customer"
            REPOSITORY_URI      = "${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com/${IMAGE_REPO_NAME}"
            S3_BUCKET           ="nebim-era-plt-deployment-yamls/nebim-era-plt-comm-customer-deployment-yaml/nebim-era-plt-comm-customer-deployment.yaml"
            SERVICE_ACCOUNT_NAME="era-plt-service-account"
            // GITHUB_SECRET       =credentials('CodeBuild/github/token')
        }

        stages {
            // // Setup Github Environment Variables
            // stage('Setup Github Environment Variables') {
            //     steps {
            //         script {
            //             def secretMap = readJSON text: GITHUB_SECRET
            //             env.GITHUB_USERNAME     = secretMap.USERNAME
            //             env.GITHUB_ACCESS_TOKEN = secretMap.TOKEN
            //             env.GITHUB_PACKAGE_URL  = secretMap.URL
            //         }
            //     }
            // }


            stage('Getting Version') {
                steps {
                    script {
                        sh '''
                        export PATH="$PATH:/var/lib/jenkins/.dotnet/tools"
                        GitVersion=$(dotnet-gitversion)
                        VERSION=$(echo $GitVersion | jq -r .NuGetVersionV2)
                        echo "VERSION=$VERSION" > version.properties
                        '''
                        def props = readProperties file: 'version.properties'
                        env.VERSION = props.VERSION
                    }
                }
            }

            // stage('Docker Build') {
            //     steps {
            //         script {
            //             withCredentials([string(credentialsId: 'Github_Token', variable: 'GITHUB_SECRET_JSON')]) {
            //                 def secretMap = readJSON text: GITHUB_SECRET_JSON
            //                 sh "docker build --build-arg GITHUB_USERNAME=${secretMap.USERNAME} --build-arg GITHUB_ACCESS_TOKEN=${secretMap.TOKEN} --build-arg GITHUB_PACKAGE_URL=${secretMap.URL} -t ${REPOSITORY_URI}:${VERSION} ."
            //             }
            //         }
            //     }
            // }
            stage('Building image') {
                steps {
                    script {
                        withCredentials([string(credentialsId: 'Github_Token', variable: 'GITHUB_SECRET_JSON')]) {
                            def secretMap = readJSON text: GITHUB_SECRET_JSON
                            sh '''
                                chmod +x docker_build.sh
                                ./docker_build.sh "${GITHUB_USERNAME}" "${GITHUB_ACCESS_TOKEN}" "${GITHUB_PACKAGE_URL}" "${REPOSITORY_URI}" "${VERSION}"
                            '''.trim()
                        }
                    }
                }
            }


            // Logging into AWS ECR
            stage('Logging into AWS ECR') {
                steps {
                    script {
                        sh "env"
                        sh "aws ecr get-login-password --region ${AWS_DEFAULT_REGION} | docker login --username AWS --password-stdin ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com"
                    }       
                }
            }

            // Cloning Git repository
            stage('Cloning Git') {
                steps {
                    checkout scmGit(branches: [[name: '*/main']], extensions: [[$class: 'WipeWorkspace']], userRemoteConfigs: [[credentialsId: 'GithubConnection', url: 'https://github.com/CihatDinc/jenkins.git']])
                }
            }
       
            // Building Docker images
            // stage('Building image') {
            //     steps{
            //         script {
            //             sh "docker build --build-arg GITHUB_USERNAME --build-arg GITHUB_ACCESS_TOKEN --build-arg GITHUB_PACKAGE_URL -t ${REPOSITORY_URI}:${VERSION} ."
            //         }
            //     }    
            // }
       
            // Uploading Docker images into AWS ECR
            stage('Pushing to ECR') {
                steps{ 
                    script {
                        sh "docker push ${REPOSITORY_URI}:${VERSION}"
                    }
                }
            }

            // Updating deployment.yaml           
            stage('Fetch and Modify deployment.yaml') {
                steps {
                    sh "aws s3 cp s3://${S3_BUCKET} ."
                    sh "cat nebim-era-plt-comm-customer-deployment.yaml"
                    sh "sed -i 's|CONTAINER_IMAGE|${REPOSITORY_URI}:${VERSION}|g' nebim-era-plt-comm-customer-deployment.yaml"
                    sh "sed -i 's|SERVICE_ACCOUNT_NAME|${SERVICE_ACCOUNT_NAME}|g' nebim-era-plt-comm-customer-deployment.yaml"
                    sh "cat nebim-era-plt-comm-customer-deployment.yaml"
                }
            }

            // Deploying to Kubernetes
            stage('K8S Deploy') {
                steps {
                    // sh "kubectl apply -f nebim-era-plt-comm-customer-deployment.yaml"
                    sh "kubectl describe deployments plt-comm-customer-deployment -n plt"
                }
            }            
        }
}
