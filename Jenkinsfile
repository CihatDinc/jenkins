pipeline {
    agent any 

    environment {
        AWS_DEFAULT_REGION = "eu-central-1"
        AWS_ACCOUNT_ID = "212845026981"
        STAGE = "plt"
        NAMESPACE = "plt"
        AWS_CLUSTER_NAME = "era-eks-dev3"
        KUBE_CONFIG = "/root/.kube/config"
        SERVICE_ACCOUNT_NAME = "era-plt-service-account"
        ECR_REPO = 'nebim-era-plt-comm-customer-dev'
        S3_BUCKET = 'nebim-era-plt-deployment-yamls/nebim-era-plt-comm-customer-deployment-yaml/nebim-era-plt-comm-customer-deployment.yaml'
        SERVICE_NAME = 'your-service-name'
        MY_SECRET = credentials('CodeBuild/github/token')
    }

    stages {
        stage('Get Version') {
            steps {
                script {
                    sh '''
                    export PATH="$PATH:/var/lib/jenkins/.dotnet/tools"
                    gitVersionOutput=$(dotnet-gitversion)
                    VERSION=$(echo $gitVersionOutput | jq -r .NuGetVersionV2)
                    echo "VERSION=$VERSION" > version.properties
                    '''
                    def props = readProperties file: 'version.properties'
                    env.VERSION = props.VERSION
                }
            }
        }

        stage('Example Stage') {
            steps {
                script {
                    echo "My Secret: ${MY_SECRET}"
                }
            }
        }

        stage('SM test') {
            steps {
                script {
                   sh '''
                   secretsValue=$(echo aws secretsmanager get-secret-value --secret-id CodeBuild/github/token)
                   username=$(echo $secretsValue | jq -r .USERNAME)
                   url=$(echo $secretsValue | jq -r .URL)
                   token=$(echo $secretsValue | jq -r .TOKEN)
                   echo "username=$username" > secrets.properties
                   echo "url=$url" >> secrets.properties
                   echo "token=$token" >> secrets.properties
                   '''
                   def props = readProperties file: 'secrets.properties'
                   env.username = props.username
                   env.url = props.url
                   env.token = props.token
                }
            }
        }


        stage('Build Docker Image') {
            steps {
                sh "docker build -t ${ECR_REPO}:${VERSION} ."
                sh "docker tag ${ECR_REPO}:${VERSION} ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com/${ECR_REPO}:${VERSION}"
                sh "aws ecr get-login-password --region ${AWS_DEFAULT_REGION} | docker login --username AWS --password-stdin ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com"
                sh "docker push ${AWS_ACCOUNT_ID}.dkr.ecr.${AWS_DEFAULT_REGION}.amazonaws.com/${ECR_REPO}:${VERSION}"
            }
        }

        // stage('.NET Build and Test') {
        //     steps {
        //         sh "dotnet nuget add source"
        //         sh "dotnet build"
        //         sh "dotnet test"
        //     }
        // }

        stage('Fetch and Modify deployment.yaml') {
            steps {
                sh "aws s3 cp s3://${S3_BUCKET} ."
                sh "cat nebim-era-plt-comm-customer-deployment.yaml"
                sh "sed -i 's/SERVICE_GIT_VERSION/${VERSION}/g' nebim-era-plt-comm-customer-deployment.yaml"
                sh "cat nebim-era-plt-comm-customer-deployment.yaml"
            }
        }

        // stage('Push NuGet Packages') {
        //     steps {
        //         sh "dotnet nuget push **/*.nupkg --source <YourNugetSource> --api-key <YourAPIKey>"
        //     }
        // }

        // stage('Push Git Changes') {
        //     steps {
        //         sh "git tag ${VERSION}"
        //         sh "git push origin ${VERSION}"
        //     }
        // }

        // stage('Apply Kubernetes Deployment') {
        //     steps {
        //         sh "kubectl apply -f deployment.yaml"
        //     }
        // }

        stage('Control Kubernetes Deployment') {
            steps {
                sh "kubectl describe deployments plt-comm-customer-deployment -n plt"
            }
        }        
    }
}
