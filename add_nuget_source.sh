#!/bin/bash

dotnet nuget add source \
    --username $GITHUB_USERNAME \
    --password $GITHUB_ACCESS_TOKEN \
    --store-password-in-clear-text \
    --name github $GITHUB_PACKAGE_URL
