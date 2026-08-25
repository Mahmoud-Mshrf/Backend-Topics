# Backend Topics

A collection of practical examples and small projects covering important **backend development and ASP.NET Core concepts**.

This repository is focused on understanding the building blocks behind modern backend applications, with an emphasis on **why a technology or pattern is used, what problem it solves, and how it behaves in a real application**.

## 📚 Topics Covered

The repository currently contains examples covering several areas of ASP.NET Core and backend development.

### 🌐 ASP.NET Core APIs

* Controller-based APIs
* Minimal APIs
* Routing
* Model Binding
* Model Validation
* API Documentation
* API Versioning

### 🧩 Application Infrastructure

* Dependency Injection
* Middleware
* Filters
* Configuration
* Background Services
* Hosted Tasks

### 🗄️ Data Access

* Entity Framework Core
* Database integration
* Working with ASP.NET Core applications and databases

### 🛡️ Security

Examples covering ASP.NET Core security concepts such as:

* Authentication
* JWT Authentication
* Authorization
* CORS
* Basic authentication/authorization concepts

### ⚠️ Error Handling

Exploration of different approaches to handling errors in ASP.NET Core applications, including centralized error-handling concepts.

### 📊 Observability

Examples and experiments around:

* Logging
* Monitoring
* Observability
* Understanding application behavior in production-like environments

### ⚡ Performance

The repository contains a dedicated performance section covering:

* Caching
* Rate limiting
* Response compression

These topics demonstrate techniques that can improve application performance, scalability, and resource usage.

### 🐳 Docker

Examples exploring how backend applications can be containerized and run using Docker.

## 🗂️ Repository Structure

```text
Backend-Topics/
│
├── API-Documentation/
├── Api-Versioning/
│   └── VersioningStrategies/
│
├── BackGroundServicesAndHostedTasks/
├── ConfigurationExplanation/
├── Controller-Based-Project/
├── DI/
├── Dockerizing/
├── Error-Handling/
├── Filters/
│
├── IntegrationWithDatabase/
│   └── WebAppWithEntityFramework/
│
├── MiddlewaresExplanation/
├── Minimal-Api-Project/
│   └── Minimal_WebApi/
│
├── ModelBinding/
├── ModelValidation/
├── Observability/
├── Performance/
│   ├── Caching/
│   ├── Rate_Limiting/
│   └── Response_Compression/
│
├── Routing/
├── SecurityInAspNetCore/
│   ├── AuthenticationWithJWT/
│   ├── BareMinimumAuthentication/
│   ├── BareMinimumAuthorization/
│   └── EnableCrossOriginResourceSharing/
│
└── BackendTopics.slnx
```

The repository is organized by topic, with each area containing focused examples or projects.

## 🎯 Purpose

The purpose of this repository is to build a strong understanding of backend development concepts through hands-on experimentation.

Rather than treating ASP.NET Core as a collection of APIs to memorize, the examples are used to explore questions such as:

* Why do we need middleware?
* When should we use filters?
* How does dependency injection work?
* How does model binding happen?
* How should validation be handled?
* What are the different approaches to API versioning?
* How should authentication and authorization be implemented?
* How can an API be made more observable?
* How can backend performance be improved?
* When should caching or compression be used?
* How can an ASP.NET Core application be containerized?

## 🧠 Learning Philosophy

The repository follows a **concept-first, hands-on approach**.

For each topic, the objective is to understand:

**Problem → Concept → Why it exists → How it works → Implementation → Trade-offs**

This makes the repository useful not only as a collection of code examples, but also as a reference while building larger ASP.NET Core applications.

## 🛠️ Technologies

* C#
* .NET / ASP.NET Core
* Entity Framework Core
* SQL Server
* Docker
* JWT
* REST APIs

## 🚀 Getting Started

Clone the repository:

```bash
git clone https://github.com/Mahmoud-Mshrf/Backend-Topics.git
```

Navigate into the repository:

```bash
cd Backend-Topics
```

Open the solution:

```bash
BackendTopics.slnx
```

Individual examples can also be run directly using the .NET CLI.

```bash
dotnet run --project <project-path>
```

Some projects may require additional configuration such as a database connection string or other environment-specific settings.

## 📖 Recommended Way to Use This Repository

If you are learning ASP.NET Core, a good approach is to study the topics progressively:

1. ASP.NET Core fundamentals
2. Routing
3. Model Binding
4. Model Validation
5. Dependency Injection
6. Middleware
7. Filters
8. Entity Framework Core
9. Error Handling
10. Authentication & Authorization
11. API Documentation
12. API Versioning
13. Background Services
14. Docker
15. Observability
16. Performance
17. Caching
18. Rate Limiting
19. Response Compression

The repository can also be used as a reference when revisiting a specific backend concept.

## 📌 Note

This is a **learning and experimentation repository**, not a single production application.

Some projects are intentionally small and isolated so that a particular ASP.NET Core or backend concept can be understood without unnecessary complexity.

The repository will continue to evolve as I learn and explore additional backend technologies and architectural concepts.

---

**Learn the concept. Understand the problem. Build the solution.**
