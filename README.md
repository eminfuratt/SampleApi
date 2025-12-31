# SampleApi – ASP.NET Core Web API

SampleApi, **ASP.NET Core Web API (.NET 8)** kullanılarak geliştirilmiş;  
**JWT tabanlı kimlik doğrulama**, **Role-Based Authorization**,  
**RabbitMQ ile Event-Driven Architecture**, **Docker** ve  
**Serilog Logging** gibi modern backend yaklaşımlarını içeren örnek bir backend uygulamasıdır.

Proje, gerçek bir üretim ortamı göz önünde bulundurularak  
**katmanlı mimari**, **clean code prensipleri** ve  
**sürdürülebilir yazılım tasarımı** dikkate alınarak geliştirilmiştir.

---
## 🎯 Proje Amacı

- Güvenli bir kullanıcı yönetimi oluşturmak  
- Ürün ve sipariş işlemlerini **rol bazlı** olarak kontrol etmek  
- Sipariş sonrası işlemleri **event-driven mimari** ile ayırmak  
- E-posta gönderimini arka planda **asenkron** olarak gerçekleştirmek  
- Dosya (JSON / CSV) üzerinden **toplu sipariş yüklemek**  
- Loglama sistemi ile tüm işlemleri kayıt altına almak  

---

## 🚀 Kullanılan Teknolojiler

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- JWT Authentication
- Role-Based Authorization (RBAC)
- MySQL
- RabbitMQ
- Docker
- SMTP (Gmail)
- Swagger (OpenAPI)
- Serilog
- Dependency Injection

---

## 🧱 Mimari Yapı

Proje **katmanlı mimari** ile geliştirilmiştir:


```text
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
```
Bu yapı sayesinde:

- Katmanlar birbirinden bağımsız çalışır
- Kod okunabilirliği ve sürdürülebilirlik artar
- Test edilebilirlik sağlanır

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
- Client tarafından `UserId` gibi kritik alanlar gönderilemez
- Rol ihlalleri engellenir
- Güvenlik açıkları önlenir

---

## 📬 Event-Driven Architecture (RabbitMQ)
Sipariş oluşturma işlemi ile e-posta gönderme işlemi birbirinden ayrılmıştır.

**Akış**
1. Sipariş veritabanına kaydedilir
2. Sipariş bilgileri RabbitMQ kuyruğuna event olarak gönderilir
3. Arka planda çalışan consumer servisi mesajı yakalar
4. E-posta işlemi asenkron olarak gerçekleştirilir

Bu sayede:

- API ana akışı bloke edilmez
- Kullanıcı daha hızlı cevap alır
- Sistem ölçeklenebilir hale gelir

## 🧪 Swagger Desteği

Swagger UI üzerinden:
- Login işlemi
- JWT token alma
- Token ile yetkili endpoint çağrıları
canlı olarak test edilebilir.

## 🧠 Mimari Kararlar

- UserId client’tan alınmaz, JWT içinden okunur
- Yetki kontrolleri Service Layer’da yapılır
- Controller’lar sadece request/response yönetir
- Admin ve User akışları net şekilde ayrılmıştır




