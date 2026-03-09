# SeeSharp.EC (E-Commerce Platform)

[English](#english) | [Tiếng Việt](#tiếng-việt)

---

## English

SeeSharp.EC is a modern, highly scalable E-Commerce backend platform built on **.NET 8** implementing **Clean Architecture**.

### 🚀 Key Features

- **Clean Architecture & Domain-Driven Design (DDD):** Clear separation into Domain, Application, Infrastructure, Persistence, and API layers.
- **CQRS Pattern:** Separation of Command and Query operations using **MediatR**.
- **Authentication & Authorization:**
  - JWT Bearer Authentication support.
  - OAuth integration (Google, GitHub, Facebook, Discord, Notion, Spotify).
  - Role-based Access Control (Admin, Employee, Subscribed, Guest).
- **Caching:** Distributed caching powered by **Redis**.
- **Message Broker:** Event-driven architecture and asynchronous messaging via **RabbitMQ** and **MassTransit**.
- **Background Jobs:** Scheduling and running background tasks via **Quartz.NET**.
- **Observability & Logging:**
  - Structured logging with **Serilog** and **Seq**.
  - Distributed Tracing and Metrics using **OpenTelemetry**.
  - Health Checks for internal services (Postgres, MySQL, RabbitMQ, Redis, etc.).
- **Storage & Media:** Integration with **Supabase** and **Cloudinary** for scalable file and image management.
- **API Gateway:** Powerful Reverse proxy configuration using **YARP (Yet Another Reverse Proxy)**.
- **Document Generation:** Generating reports and invoices in PDF format with **QuestPDF**.
- **Local Environment (Docker):** Pre-configured `docker-compose.yml` for an easy local development setup.

### 🏗️ Tech Stack & Libraries

- **Core Framework:** .NET 8, ASP.NET Core (Minimal APIs & Controllers)
- **ORMs:** Entity Framework Core (MySQL / PostgreSQL support)
- **Data & Message Queue:** Redis, RabbitMQ
- **Validation:** FluentValidation
- **API Documentation:** Swagger / Scalar OpenAPI
- **Testing:** xUnit, coverlet.collector
- **Others:** FluentEmail.Smtp, NaughtyStrings, Ulid, etc.

### 📂 Project Structure

- `API` - Presentation layer containing endpoints, controllers, and middleware configuration.
- `Application` - Contains business logic, CQRS handlers (MediatR), validation, and abstractions/interfaces.
- `Domain` - Core business models, entities, exceptions, and domain events.
- `Infrastructure` - External communication components and service implementations (Email via Papercut-SMTP, Storage, Message Broker, OAuth).
- `Persistence` - Contains EF Core DbContext, migrations, and repository implementations.
- `SharedKernel` - Utilities, constants, and shared types used across all layers.
- `Yarp.ReverseProxy` - API Gateway configuration using YARP.

### 🐳 Local Development Guide

**Prerequisites:**
- [Docker](https://www.docker.com/) & [Docker Compose](https://docs.docker.com/compose/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

**1. Clone the repository:**
```bash
git clone <repository_url>
cd SeeSharp.EC
```

**2. Configure Environment Variables (`.env`):**
Copy `.env.example` to `.env` at the root directory and set the necessary environment variables (e.g., `Seq` password, `Google/Github ClientID`, etc.).

**3. Run Infrastructure Services:**
Execute the following command to spin up required Docker containers (Redis, Seq, Papercut SMTP, etc.):
```bash
docker-compose up -d
```
*(Note: You can uncomment or modify the MySQL/Postgres/RabbitMQ blocks in the `docker-compose.yml` file as needed).*

**4. Run the API Project:**
```bash
cd API
dotnet run
```

**5. Access Services:**
- **Swagger/Scalar UI (API Docs):** `https://localhost:<port>/scalar` or `https://localhost:<port>/swagger`
- **Seq (Log Server):** `http://localhost:5341`
- **Papercut SMTP (Email Testing):** `http://localhost:8080`

### 🧪 Running Unit Tests

Make sure you are in the root directory or the folder containing the `.sln` file, and run:
```bash
dotnet test
```

### 📄 License

This project is licensed under the MIT License or the development team's proprietary license.

---

## Tiếng Việt

SeeSharp.EC là một nền tảng E-Commerce backend hiện đại, có khả năng mở rộng cao, được xây dựng trên **.NET 8** sử dụng kiến trúc **Clean Architecture**.

### 🚀 Các tính năng chính (Features)

- **Clean Architecture & Domain-Driven Design (DDD):** Tách biệt các layer thành Domain, Application, Infrastructure, Persistence và API.
- **CQRS Pattern:** Thực hiện tách biệt Command và Query thông qua **MediatR**.
- **Xác thực và Phân quyền (Authentication & Authorization):**
  - Hỗ trợ JWT Bearer Authentication.
  - Tích hợp OAuth (Google, GitHub, Facebook, Discord, Notion, Spotify).
  - Phân quyền theo Role-based (Admin, Employee, Subscribed, Guest).
- **Caching:** Distributed caching bằng **Redis**.
- **Message Broker:** Kiến trúc hướng sự kiện (Event-driven) và giao tiếp bất đồng bộ qua **RabbitMQ** cùng **MassTransit**.
- **Background Jobs:** Lên lịch và chạy các tác vụ ngầm bằng **Quartz.NET**.
- **Observability & Logging:**
  - Ghi log có cấu trúc (Structured logging) với **Serilog** và **Seq**.
  - Distributed Tracing và Metrics sử dụng **OpenTelemetry**.
  - Health Checks cho các dịch vụ nội bộ (Postgres, MySQL, RabbitMQ, Redis, v.v.).
- **Storage & Media:** Tích hợp với **Supabase** và **Cloudinary** để quản lý file và hình ảnh một cách tối ưu.
- **API Gateway:** Cấu hình Reverse proxy mạnh mẽ bằng **YARP (Yet Another Reverse Proxy)**.
- **Document Generation:** Tạo báo cáo và hóa đơn định dạng PDF dùng **QuestPDF**.
- **Môi trường cục bộ (Docker):** Đã cấu hình sẵn `docker-compose.yml` để dễ dàng chạy môi trường phát triển nội bộ.

### 🏗️ Tech Stack & Thư viện sử dụng

- **Framework chính:** .NET 8, ASP.NET Core (Hỗ trợ Minimal APIs / Controllers)
- **ORMs:** Entity Framework Core (Hỗ trợ MySQL / PostgreSQL)
- **Data & Message Queue:** Redis, RabbitMQ
- **Validation:** FluentValidation
- **Tài liệu API:** Swagger / Scalar OpenAPI
- **Testing:** xUnit, coverlet.collector
- **Khác:** FluentEmail.Smtp, NaughtyStrings, Ulid, v.v.

### 📂 Cấu trúc dự án (Project Structure)

- `API` - Lớp Presentation chứa endpoints, controllers, và cấu hình middleware.
- `Application` - Chứa business logic, CQRS handlers (dùng MediatR), validation, và các interfaces abstraction.
- `Domain` - Core business models, entities, exceptions và domain events.
- `Infrastructure` - Các thành phần giao tiếp bên ngoài và dịch vụ thực thi (Email qua Papercut-SMTP, Storage, Message Broker, OAuth).
- `Persistence` - Lớp chứa EF Core DbContext, migrations, và các repository implementations.
- `SharedKernel` - Tiện ích, hằng số (constants) và types dùng chung cho tất cả các layer.
- `Yarp.ReverseProxy` - Cấu hình API Gateway sử dụng YARP.

### 🐳 Hướng dẫn chạy môi trường cục bộ (Local Development)

**Yêu cầu môi trường:**
- [Docker](https://www.docker.com/) & [Docker Compose](https://docs.docker.com/compose/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

**1. Clone the repository:**
```bash
git clone <repository_url>
cd SeeSharp.EC
```

**2. Cấu hình biến môi trường (`.env`):**
Copy file `.env.example` thành `.env` tại thư mục root và cài đặt các biến môi trường cần thiết (ví dụ: mật khẩu cho `Seq`, thông tin `Google/Github ClientID`, v.v.).

**3. Khởi chạy các dịch vụ Infrastructure:**
Chạy lệnh sau để bật các dịch vụ docker container cần thiết (Redis, Seq, Papercut SMTP, v.v.):
```bash
docker-compose up -d
```
*(Lưu ý: Bạn có thể bỏ comment hoặc sửa đổi block phần MySql/Postgres/Rabbitmq trong tập tin `docker-compose.yml` tùy nhu cầu thực tế).*

**4. Chạy dự án:**
```bash
cd API
dotnet run
```

**5. Truy cập các dịch vụ:**
- **Swagger/Scalar UI (Tài liệu API):** `https://localhost:<port>/scalar` hoặc `https://localhost:<port>/swagger`
- **Seq (Xem Log Server):** `http://localhost:5341`
- **Papercut SMTP (Test gửi Email):** `http://localhost:8080`

### 🧪 Chạy Unit Test

Đảm bảo bạn đứng ở thư mục gốc hoặc thư mục chứa solution `.sln`, và chạy lệnh:
```bash
dotnet test
```

### 📄 License (Chính sách cấp phép)

Dự án này tuân thuộc theo MIT License hoặc chính sách bản quyền riêng của nhóm phát triển.
