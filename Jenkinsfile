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
    }

    stages {
        stage('Get Version') {
            steps {
                script {
                    sh "export PATH="$PATH:/var/lib/jenkins/.dotnet/tools""
                    def gitVersionOutput = sh(script: "dotnet-gitversion", returnStdout: true).trim()
                    VERSION = sh(script: "echo '${gitVersionOutput}' | jq -r .NuGetVersionV2", returnStdout: true).trim()
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
