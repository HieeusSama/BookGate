# 📚 BookGate - Nền Tảng Bán Sách & Quản Lý Cửa Hàng Trực Tuyến

[![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET_Core_MVC-512BD4?style=for-the-badge&logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/mvc)
[![SQL Server](https://img.shields.io/badge/SQLServer-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![Clean Architecture](https://img.shields.io/badge/Clean_Architecture-222222?style=for-the-badge&logo=data:image/png;base64,...)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

> **BookGate** là một website quản lý bán hàng trực tuyến toàn diện dành cho cửa hàng sách. Dự án cung cấp nền tảng giúp khách hàng dễ dàng tìm kiếm, mua sắm sách, đồng thời cung cấp công cụ mạnh mẽ để quản trị viên (Admin) kiểm soát sản phẩm, kho hàng và đơn hàng một cách hiệu quả.

🌐 **Live Demo:** [bookgate.tryasp.net](http://bookgate.tryasp.net/)  
### 🔐 Tài khoản trải nghiệm (Demo Account)
Để kiểm tra các tính năng quản trị, bạn có thể đăng nhập bằng tài khoản sau:
* **Tài khoản (Email):** `admin@gmail.com`
* **Mật khẩu:** `1111`

👨‍💻 **Vị trí phát triển:** Full Stack Developer

---

## ✨ Tính năng nổi bật

### 🛒 Dành cho Khách hàng & Mua sắm
* **Giao diện trực quan:** Trải nghiệm UI/UX nhất quán, thân thiện trên đa thiết bị (Responsive design).
* **Logistics Thông Minh (Tích hợp GHN):** Kết nối API Giao Hàng Nhanh (GHN) để tự động tính toán phí vận chuyển chính xác dựa trên tọa độ và vùng miền, tối ưu hóa quy trình Checkout.
* **Thanh Toán Trực Tuyến An Toàn:** Tích hợp cổng thanh toán **VNPay**. Xử lý quy trình thanh toán bảo mật, quản lý trạng thái giao dịch tự động qua IPN (Instant Payment Notification).
* **🤖 Trợ lý ảo AI (AI Assistant):** Tích hợp Google AI Studio (**Gemini API**) đóng vai trò như một nhân viên tư vấn ảo, hỗ trợ người dùng tìm kiếm và chọn lựa sách dựa trên nhu cầu, sở thích cá nhân.

### ⚙️ Dành cho Quản trị viên (Admin)
* Quản lý toàn diện danh mục sản phẩm, đầu sách, tác giả và nhà xuất bản.
* Theo dõi và kiểm soát kho hàng theo thời gian thực.
* Quản lý và theo dõi trạng thái đơn hàng từ lúc đặt đến lúc giao thành công.

---

## 🛠 Công nghệ sử dụng

Dự án được xây dựng với các tiêu chuẩn thiết kế phần mềm hiện đại nhằm đảm bảo hiệu suất, dễ bảo trì và khả năng mở rộng cao.

### 🏛 Kiến trúc hệ thống
* **Clean Architecture:** Phân tách hệ thống thành các layer độc lập (Domain, Application, Infrastructure, Web/UI), tuân thủ chặt chẽ các nguyên tắc SOLID.
* **Design Patterns:**
  * **Generic Repository Pattern:** Tối ưu hóa truy xuất cơ sở dữ liệu và tái sử dụng code.
  * **Dependency Injection (DI):** Quản lý dependencies hiệu quả, giảm thiểu sự phụ thuộc giữa các class.
* **Data Mapping:** Sử dụng **DTOs (Data Transfer Objects)** và **AutoMapper** để ánh xạ dữ liệu an toàn, bảo mật thông tin thực thể (Entities) khi giao tiếp với UI.

### 💻 Backend
* **Ngôn ngữ:** C#
* **Framework:** ASP.NET Core MVC
* **ORM:** Entity Framework Core

### 🎨 Frontend
* HTML5, CSS3, JavaScript
* Bootstrap (Responsive & UI components)

### 🗄 Cơ sở dữ liệu
* Microsoft SQL Server

### 🔌 Tích hợp (3rd Party APIs)
* **Giao Hàng Nhanh (GHN) API:** Quản lý vận chuyển và tính phí ship.
* **VNPay API:** Cổng thanh toán điện tử.
* **Google Gemini API:** Xử lý ngôn ngữ tự nhiên cho AI Assistant.

---

## 🚀 Hướng dẫn cài đặt (Local Development)

Để chạy dự án BookGate trên máy cá nhân, vui lòng làm theo các bước sau:

### Yêu cầu hệ thống
* [.NET SDK 8.0](https://dotnet.microsoft.com/download) (Hoặc phiên bản tương ứng bạn sử dụng)
* Microsoft SQL Server
* Visual Studio 2022 hoặc Visual Studio Code

### Các bước thực hiện
1. **Clone repository:**
   ```bash
   git clone [https://github.com/HieeusSama/BookGate.git](https://github.com/HieeusSama/BookGate.git)
   cd BookGate
