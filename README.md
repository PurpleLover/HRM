# HRM (Human Resource Management)

## Giới thiệu

Phần mềm quản lý nhân sự cho doanh nghiệp. Viết trên C# .NET. Vào thời điểm viết, nó mới chỉ có hạng mục tuyển dụng, sẽ bổ sung thêm sau này.

## Cấu trúc thư mục

* Model: chứa các code để tạo ra Model gốc lên Database
* Repository: trung gian liên kết Model với Service
* Service: chứa các code để kết nói giữa Model với Web, các hàm lấy danh sách, so sánh viết ở đây
* Web: chứa các phương thức tương tác với người dùng

Ngoài ra,

* CommonHelper: chứa các phương thức chung có thể tái sử dụng, như là lấy thứ trong tiếng Việt
