* What is difference among 'Continuous Integration', 'Continuous Delivery', 'Continuous Deployment'?

Azure Pipelines: 
Pipelines provide a way to build and deploy your software. 

<img width="571" alt="image" src="https://github.com/prabhanjanduholiya/learn.azure/assets/31764786/3d42927f-b45f-40b8-b073-c22ee52fa6aa">


## Continuous Integration (CI):

**Definition:**
A development practice where developers integrate code changes into a shared repository as often as possible, ideally multiple times a day. 

**Purpose:**
To identify integration issues early, ensuring code quality and preventing integration hell during the release process. 

**Key Activities:**
Automated builds, unit tests, integration tests, and other quality checks triggered by code commits. 

**Goal:**
To make sure the software is always in a deployable state, even with frequent changes. 

## Continuous Delivery/Deployment (CD):

**Definition:**
Extends CI by automating the release process, ensuring that code changes are automatically deployed to testing and production environments. 

**Continuous Delivery:**
Focuses on automating the release process up to the point of deployment, but a manual step might be involved before releasing to production. 

**Continuous Deployment:**
Goes a step further by automating the deployment to production environments without manual intervention. 

**Key Activities:**
Automated deployment to various environments, automated acceptance tests, and other checks to ensure readiness for release. 

**Goal:**
To make software releases frequent, reliable, and predictable, reducing manual effort and release cycles. 
In essence: CI is about integrating code changes and verifying them, while CD is about automating the delivery and deployment of those changes to users. 



