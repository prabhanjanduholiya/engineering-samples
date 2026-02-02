# Architecture

# AWS 

## AWS EC2
* What are public, private and elastic IP addresses?
* What is the difference between IPV4 and IPV6 addresses?
* Can restart of EC2 change the public address, how you fix this if IP is consumed outside?
* What are EC2 placement groups?
  - Placement groups define how your EC2 machines are physically organized in availavility zones. Stragtegies to define placement groups are
  - Clusters (All instances in same AZ), Spread (across multiple AZ) ,Partitions (Spread aross racks into an AZ while spread acorss multiple AZ)
* What is Elastic Network Interfaces (ENI)?
  - An ENI has associated public and private IP address ranges, you can attach these ENI to EC2 instances and you can also change ENI in case of failovers.
    
## API Gateway , Load Balancers & Reverse Proxy 
* What is diffrence between Reverse Proxy, API Gateway and Load Balancers?
* What are the load balancer algorthims?
