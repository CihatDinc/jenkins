pipeline {
    agent {
        dockerContainer {
            image 'docker pull docker:dind'
        }
    }

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

            stage ('Test'){
                steps {
                    sh '''
                    apt-get update -y
                    apt-get install sudo -y
                    apt-get install -y dotnet-sdk-7.0
                    dotnet tool install --global GitVersion.Tool --version 5.*
                    PATH="/app/.dotnet/tools:${PATH}"
                    '''
                }
            }

            stage('Login to AWS ECR'){
                steps {
                    script {
                       // AWS ECR ile kimlik doğrulama
                       sh "aws ecr get-login-password --region ${AWS_DEFAULT_REGION} | docker login --username AWS --password-stdin ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com"
                   }
                }
            }

            stage('Install'){
                steps {
                    script {
                        sh '''
                        apt-update -y
                        apt install sudo -y
                        sudo apt install zip
                        curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
                        unzip awscliv2.zip
                        sudo ./aws/install --bin-dir /usr/local/bin --install-dir /usr/local/aws-cli --update
                        aws --version
                        apt install jq -y
                        wget https://get.helm.sh/helm-v3.7.2-linux-amd64.tar.gz -O helm.tar.gz; tar -xzf helm.tar.gz
                        chmod +x ./linux-amd64/helm
                        mv ./linux-amd64/helm /usr/local/bin/helm
                        curl -o kubectl https://amazon-eks.s3.us-west-2.amazonaws.com/1.18.9/2020-11-02/bin/linux/amd64/kubectl   
                        chmod +x ./kubectl
                        mkdir -p $HOME/bin && cp ./kubectl $HOME/bin/kubectl && export PATH=$PATH:$HOME/bin
                        echo 'export PATH=$PATH:$HOME/bin' >> ~/.bashrc
                        curl -o aws-iam-authenticator https://amazon-eks.s3-us-west-2.amazonaws.com/1.10.3/2018-07-26/bin/linux/amd64/aws-iam-authenticator
                        chmod +x ./aws-iam-authenticator
                        cp ./aws-iam-authenticator $HOME/bin/aws-iam-authenticator && export PATH=$HOME/bin:$PATH
                        echo 'aws-iam-authenticator installed'  
                        echo 'Check kubectl version'
                        kubectl version --short --client
                        '''
                    }
                }
            }

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
