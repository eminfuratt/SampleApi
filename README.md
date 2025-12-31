# SampleApi – ASP.NET Core Web API

Bu proje, ASP.NET Core Web API kullanılarak geliştirilmiş, **JWT tabanlı kimlik doğrulama**,  
**Role-Based Access Control (RBAC)**, **DTO Pattern**, **Repository & Service Layer mimarisi**
üzerine kurulmuş örnek bir backend uygulamasıdır.

Proje, gerçek bir üretim ortamına uygun olacak şekilde **katmanlı mimari** ve
**clean code prensipleri** dikkate alınarak tasarlanmıştır.

---

## 🚀 Kullanılan Teknolojiler

- ASP.NET Core Web API
- Entity Framework Core
- JWT Authentication
- Role-Based Authorization (RBAC)
- MySQL
- Swagger (OpenAPI)
- Repository Pattern
- Service Layer
- DTO Pattern
- Dependency Injection

---

## 🧱 Mimari Yapı

Proje **katmanlı mimari** ile geliştirilmiştir:
Controllers
│
├── Services
│   └── İş kuralları ve yetki kontrolleri
│
├── Repositories
│   └── Veri tabanı erişimi
│
├── DTOs
│   └── API sözleşmesi ve veri transferi
│
└── Models (Entities)
    └── Veritabanı modelleri



---

## 🎯 Kullanılan Design Pattern’ler

### ✅ Repository Pattern
- Veri tabanı erişimi soyutlandı
- EF Core bağımlılığı servislerden ayrıldı
- Test edilebilirlik artırıldı

### ✅ Service Layer
- İş kuralları controller’dan ayrıldı
- Yetki kontrolleri merkezi hale getirildi
- Controller’lar sadeleştirildi

### ✅ DTO Pattern
- Entity’ler doğrudan dış dünyaya açılmadı
- Güvenlik ve API sözleşmesi sağlandı
- Rol bazlı veri kontrolü yapıldı

### ✅ Dependency Injection
- Loose Coupling sağlandı
- Mock repository ile test yapılabilir hale getirildi

---

## 🔐 Kimlik Doğrulama & Yetkilendirme

### JWT Authentication
- Login sonrası JWT token üretilir
- Token içerisinde:
  - UserId
  - Email
  - Role bilgileri bulunur

### Role-Based Access Control (RBAC)

| Rol | Yetkiler |
|----|---------|
| **Admin** | Tüm siparişleri görür, oluşturur, günceller, siler |
| **User** | Sadece kendi siparişlerini görür ve yönetir |

Yetkilendirme `[Authorize]` ve `[Authorize(Roles = "Admin")]` attribute’ları ile sağlanmıştır.

---

## 📦 DTO Kullanımı

DTO’lar, API ile client arasındaki veri sözleşmesini belirler.

Örnek:
- `CreateOrderDto` → Kullanıcı sipariş oluşturma
- `CreateOrderByAdminDto` → Admin başka kullanıcı adına sipariş oluşturma
- `UpdateOrderDto` → Sipariş güncelleme

Bu sayede:
- User, `UserId` gönderemez
- Rol ihlalleri engellenir
- Güvenlik açıkları önlenir

---

## 🧪 Swagger Desteği

Swagger UI üzerinden:
- Login işlemi
- JWT token alma
- Token ile yetkili endpoint çağrıları
canlı olarak test edilebilir.


---

## 📌 Örnek Endpoint’ler

POST /api/auth/login
GET /api/orders/my-orders
POST /api/orders
POST /api/orders/admin
PUT /api/orders/my-order/{id}
DELETE /api/orders/{id}


---

## 🧠 Mimari Kararlar

- UserId client’tan alınmaz, JWT içinden okunur
- Yetki kontrolleri Service Layer’da yapılır
- Controller’lar sadece request/response yönetir
- Admin ve User akışları net şekilde ayrılmıştır




