# AWS Secret Manager

Goal is your application should be able to pull/manage secrets from secret manager.

To grant an IAM role access to AWS Secrets Manager, create an IAM role, define a permissions policy allowing secretsmanager:GetSecretValue (and other actions like DescribeSecret or PutSecretValue), specify the secret ARN(s) as the resource, and attach this policy to the role, then associate the role with your service (like EC2, Lambda, or EKS Pods). Best practice involves using least privilege by restricting permissions to specific secrets and actions, often using tags or specific ARNs. 

**Steps to Grant Permissions**
Create an IAM Policy: Define what actions are allowed and on which resources (secrets).
Example (JSON) for a specific secret:
```
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": "secretsmanager:GetSecretValue",
            "Resource": "arn:aws:secretsmanager:REGION:ACCOUNT-ID:secret:YOUR-SECRET-NAME-XXXXXX"
        },
        {
            "Effect": "Allow",
            "Action": "secretsmanager:DescribeSecret",
            "Resource": "arn:aws:secretsmanager:REGION:ACCOUNT-ID:secret:YOUR-SECRET-NAME-XXXXXX"
        }
    ]
}
```
**Create an IAM Role:**

* Go to the IAM console, click Roles, and Create role.
* Select the service that will assume the role (e.g., EC2, Lambda, EKS).
* Attach the policy you created (or a managed policy like SecretsManagerReadWrite for broad access, though less secure).

* Attach the Role to Your Service:
    - EC2: Attach the role to the instance profile.
    - Lambda/ECS: Assign the role as the execution role.
    - EKS: Use IAM Roles for Service Accounts (IRSA) to map the role to a Kubernetes Service Account. 

**Key Concepts**

* Least Privilege: Grant only the necessary permissions (secretsmanager:GetSecretValue, etc.) and only for specific secrets (using ARN or tags).
* Identity-Based Policies: Attach policies directly to users, groups, or roles (what you're doing here).
* Resource-Based Policies: Used for cross-account sharing or granting access from other AWS services directly on the secret.
* KMS Keys: Secrets are encrypted by KMS keys; the role also needs kms:Decrypt permissions if a customer-managed key is used. 


# Azure Key Vault
