pipeline {
    agent any
        environment {
            AWS_ACCOUNT_ID      ="212845026981"
            AWS_DEFAULT_REGION  ="eu-central-1" 
            IMAGE_REPO_NAME     ="jenkins-test-customer"
            REPOSITORY_URI      = "${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com/${IMAGE_REPO_NAME}"
            S3_BUCKET           ="nebim-era-plt-deployment-yamls/nebim-era-plt-comm-customer-deployment-yaml/nebim-era-plt-comm-customer-deployment.yaml"
            SERVICE_ACCOUNT_NAME="era-plt-service-account"
            GITHUB_SECRET       =credentials('CodeBuild/github/token')
            GITHUB_ACCESS_TOKEN =credentials('Github_Token')
        }

        stages {
            stage('Getting Version') {
                steps {
                    script {
                        sh '''
                        env
                        export PATH="$PATH:/var/lib/jenkins/.dotnet/tools"
                        GitVersion=$(dotnet-gitversion)
                        VERSION=$(echo $GitVersion | jq -r .NuGetVersionV2)
                        echo "VERSION=$VERSION" > version.properties
                        '''
                        def props = readProperties file: 'version.properties'
                        env.VERSION = props.VERSION
                        env.VERSIONTAG = "0.0.${BUILD_NUMBER}-${GIT_PREVIOUS_COMMIT}-${VERSION}"
                    }
                }
            }

            stage('Docker Build') {
                steps {
                    script {
                        withCredentials([string(credentialsId: 'CodeBuild/github/token', variable: 'GITHUB_INFO')]) {
                            def secretMap = readJSON text: GITHUB_INFO
                            sh "docker build --build-arg GITHUB_USERNAME=${secretMap.USERNAME} --build-arg GITHUB_ACCESS_TOKEN=${GITHUB_ACCESS_TOKEN} --build-arg GITHUB_PACKAGE_URL=${secretMap.URL} -t ${REPOSITORY_URI}:${VERSIONTAG} ."
                        }
                    }
                }
            }

            // Logging into AWS ECR
            stage('Logging into AWS ECR') {
                steps {
                    script {
                        sh "aws ecr get-login-password --region ${AWS_DEFAULT_REGION} | docker login --username AWS --password-stdin ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com"
                    }       
                }
            }
       
            // Uploading Docker images into AWS ECR
            stage('Pushing to ECR') {
                steps{ 
                    script {
                        sh "docker push ${REPOSITORY_URI}:${VERSIONTAG}"
                    }
                }
            }

            // Updating deployment.yaml           
            stage('Fetch and Modify deployment.yaml') {
                steps {
                    sh "aws s3 cp s3://${S3_BUCKET} ."
                    sh "cat nebim-era-plt-comm-customer-deployment.yaml"
                    sh "sed -i 's|CONTAINER_IMAGE|${REPOSITORY_URI}:${VERSIONTAG}|g' nebim-era-plt-comm-customer-deployment.yaml"
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
