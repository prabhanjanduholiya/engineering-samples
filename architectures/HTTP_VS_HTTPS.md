### Web Security:
* What is SSL or TLS, How it works?
* What is difference between Http and Https?
* How TLS version differs?

## **What Is SSL/TLS? **

SSL, or Secure Sockets Layer (now largely superseded by TLS), establishes a secure, encrypted connection between a client and a server, preventing man in the middle attacks during online communication. 

It uses a combination of asymmetric and symmetric encryption to achieve this security. 

### **Difference Between TLS and SSL**
The original security protocol for [HTTP](https://www.geeksforgeeks.org/different-kinds-of-http-requests/) was SSL, which has since been replaced by TLS. Although the SSL name is still commonly used, SSL handshakes are now referred to as TLS handshakes.

## **How SSL Works?**

Here's a breakdown of how it works:
### 1. The Handshake:
**Initiation:**
When a client (like a web browser) connects to a website with SSL enabled, it initiates a "handshake" with the server. 

**Server Identification:**
The server sends its SSL certificate, which includes its public key and information identifying the website. 

**Certificate Verification:**
The client verifies the certificate by checking its validity and whether it matches the website's domain. This verification often involves checking with trusted Certificate Authorities (CAs). 

**Key Exchange:**
Once the certificate is verified, the client generates a random session key (symmetric key) and encrypts it using the server's public key. 

**Session Key Exchange:**
This encrypted session key is then sent to the server. 

### 2. Secure Communication:

**Decryption:**
The server uses its private key to decrypt the message and obtain the session key. 

**Encrypted Communication:**
From this point on, both the client and server use the symmetric session key to encrypt and decrypt all data transmitted between them. 

### 3. Key Concepts:
**Asymmetric Encryption:**
Uses a pair of keys (public and private) for encryption and decryption. The public key is used for encryption, and the private key is kept secret for decryption. 

**Symmetric Encryption:**
Uses the same key for both encryption and decryption. This is faster and more efficient for encrypting large amounts of data. 

**SSL/TLS Certificates:**
Digital certificates issued by trusted CAs that verify the identity of a website and enable secure communication. 

**Certificate Authorities (CAs):**
Trusted third-party organizations that issue and manage SSL/TLS certificates. 

In essence, SSL/TLS uses a combination of asymmetric and symmetric encryption to establish a secure, authenticated, and encrypted connection. This ensures that data transmitted between a client and a server remains confidential and protected from tampering. 

**What is the difference between HTTP and HTTPS?**
The S in "HTTPS" stands for "secure." HTTPS is just HTTP with SSL/TLS. A website with an HTTPS address has a legitimate SSL certificate issued by a certificate authority, and traffic to and from that website is authenticated and encrypted with the SSL/TLS protocol.

To encourage the Internet as a whole to move to the more secure HTTPS, many web browsers have started to mark HTTP websites as "not secure" or "unsafe." Thus, not only is HTTPS essential for keeping users safe and user data secure, it has also become essential for building trust with users.

## **Key differences Between TLS 1.2 and TLS 1.3** 
TLS 1.3 offers significant improvements over TLS 1.2, primarily in the areas of security and performance. TLS 1.3 uses stronger, more modern cipher suites and simplifies the handshake process, resulting in faster connection times and enhanced security. Older versions, TLS 1.0 and 1.1, are now deprecated due to known vulnerabilities. 

Here's a more detailed breakdown:

### Security:
**Stronger Cipher Suites:**
TLS 1.3 mandates the use of more secure cryptographic algorithms, eliminating the potential for vulnerabilities associated with weaker ciphers used in older versions. 

**Improved Handshake:**
The handshake process in TLS 1.3 is streamlined, reducing the number of packets exchanged and thus minimizing the attack surface. 

**Removal of Weak Algorithms:**
TLS 1.3 removes support for algorithms with known vulnerabilities, such as those found in TLS 1.2 and earlier. 

**Zero Round-Trip Time (0-RTT):**
In some cases, TLS 1.3 can establish a secure connection with zero round trips, further enhancing performance and security. 

### Performance:
**Faster Handshake:**
TLS 1.3's streamlined handshake process results in faster connection establishment compared to TLS 1.2.

**Reduced Round Trips:**
The number of round trips required for a handshake is reduced in TLS 1.3, leading to lower latency and quicker data transfer.

**Optimized Algorithms:**
TLS 1.3 utilizes more efficient cryptographic algorithms, contributing to faster encryption and decryption. 

### References: 
**How SSL Works?** 
Well explained in detail - 
https://www.youtube.com/watch?v=0yw-z6f7Mb4 


-----------------------


### HTTP vs HTTPS
HTTPS is HTTP with encryption. The difference between the two protocols is that HTTPS uses TLS (SSL) to encrypt normal HTTP requests and responses. As a result, HTTPS is far more secure than HTTP. 

**HTTP**

HTTP stands for Hypertext Transfer Protocol, and it is a protocol—or a prescribed order and syntax for presenting information—used for transferring data over a network. Most information that is sent over the Internet, including website content and API calls, uses the HTTP protocol.

There are two main kinds of HTTP messages: requests and responses. HTTP requests are generated by a user's browser as the user interacts with web properties. For example, if a user clicks on a hyperlink, the browser will send a series of "HTTP GET" requests for the content that appears on that page. These HTTP requests go to either an origin server or a proxy caching server, and that server will generate an HTTP response. HTTP responses are answers to HTTP requests.

HTTP requests and responses are sent across the Internet in plaintext. The problem is that anyone monitoring the connection can read these plaintexts. This is especially an issue when users submit sensitive data via a website or a web application. This could be a password, a credit card number, or any other data typed into a form. Essentially, a malicious actor can just read the text in the request or the response and know exactly what information someone is asking for, sending, or receiving, and even manipulate the communication.

The answer to above security problem is HTTPS.

**HTTPS**

HTTPS stands for Hypertext Transfer Protocol Secure (also referred to as HTTP over TLS or HTTP over SSL). HTTPS uses TLS (or SSL) to encrypt HTTP requests and responses, so instead of the plaintext, an attacker would see a series of seemingly random characters.

TLS uses a technology called public key encryption: there are two keys, a public key and a private key. The public key is shared with client devices via the server's SSL certificate. The certificates are cryptographically signed by a Certificate Authority (CA), and each browser has a list of CAs it implicitly trusts. Any certificate signed by a CA in the trusted list is given a green padlock lock in the browser’s address bar, because it is proven to be “trusted” and belongs to that domain. Companies like Let’s Encrypt have now made the process of issuing SSL/TLS certificates free.

When a client opens a connection with a server, each machine needs a verified identity. So, the two devices use the public and private key to agree on new keys, called session keys, to encrypt further communications between them. All HTTP requests and responses are then encrypted with these session keys, so that anyone who intercepts communications can only see a random string of characters, not the plaintext.

A session key is an [encryption](https://www.techtarget.com/searchsecurity/definition/encryption) and decryption [key](https://www.techtarget.com/searchsecurity/definition/key) that is randomly generated to ensure the security of a communications session between a user and another computer or between two computers. Session keys are sometimes called symmetric keys because the same key is used for both encryption and decryption.

The public key is truly public and can be shared widely while the private key should be known only to the owner. In order for a client to establish a secure connection with a server, it first checks the server’s digital certificate. Then, the client generates a session key that it encrypts with the server’s public key. The server decrypts this session key with its private key (that’s known only to the server), and the session key is used by the client-server duo to encrypt and decrypt messages in that session.

In addition to encrypting communication, HTTPS is used for authenticating the two communicating parties. Authentication means verifying that a person or machine is who they claim to be. In HTTP, there is no verification of identity—it is based on a principle of trust. But on the modern Internet, authentication is essential.

