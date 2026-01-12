# 🛒 E-Commerce Administrative System

Sistema administrativo y de usuario para un **E‑Commerce moderno**, con autenticación por roles, panel de administración, estadísticas y experiencia completa de compra.

---

## 🔐 Credenciales de prueba

### Administrador

* **Email:** [ignacio@admin.com](mailto:ignacio@admin.com)
* **Password:** admin123

### Cliente

* **Email:** [cliente@cliente.com](mailto:cliente@cliente.com)
* **Password:** cliente123

---

## 🧱 Stack Tecnológico

### Backend

* **Framework:** .NET 8.0 – Web API (v8.0.7)
* **ORM:** Entity Framework Core
* **Base de datos:** PostgreSQL
* **Proveedor EF:** Npgsql v9.0.4
* **Autenticación:** JWT Bearer v8.0.4
* **Hashing:** BCrypt.Net‑Next v4.0.3

### Frontend

* **Framework:** Vue 3 (v3.5.22)
* **Routing:** Vue Router v4.5.1
* **Estado global:** Pinia v3.0.3
* **Lenguaje:** TypeScript v5.9.0
* **Estilos:** Tailwind CSS v3.4.13
* **Compatibilidad:** Autoprefixer v10.4.20 + PostCSS v8.4.31
* **Gráficos:** Chart.js v4.5.1 + vue-chartjs v5.3.3

---

## 📋 Work List / Progreso

### Funcionalidades generales

* ✅ Paginación frontend y backend (page por parámetro)
* ✅ Cambio de estado de órdenes (Pagado / Enviado / Cancelado)
* ✅ Cantidad total de órdenes y monto total gastado

### AdminStats

* ✅ Ventas totales
* ✅ Ventas mensuales
* ✅ Órdenes por estado
* ✅ Top productos
* ✅ Usuarios nuevos por mes

### Usuarios

* ⏳ Bloquear / desactivar usuario
* ⏳ Panel de usuario (ver órdenes)

---

## 🏠 Módulos Home – Usuario

-  ❌ (on progress)
* ⭐ Featured products / Productos destacados
* 🔍 Filters / Filtros
* 📦 Product Details / Detalle de producto
* 🛒 Buy Cart / Carrito de compras
* 💳 Checkout
* 🔑 Login / Registrer / Registro
* ⏳ User panel / Panel de usuario (ver órdenes) *(en progreso)*

---

## 🛠️ Módulos Home – Administrador

* 🔐 Login Admin / Register Admin / Registro de administrador
* 📦 Product Managment / Gestión de productos
* 👥 User Managment / Gestión de usuarios
* 📑 Order Managment / Gestión de órdenes
* 📊 Statistics and graphs / Estadísticas y gráficos

---

## ⚙️ Backend – Funcionalidades

* CRUD completo:

  * Products / Productos 
  * Categories / Categorías
  * Users / Usuarios
  * Orders / Órdenes
  * Order Details (products) / Detalles de orden (productos)

* 📊 Estadísticas:

  * Total Sales / Ventas totales ,, , , 
  * Stats by Order state / Ventas por estado de orden
  * Mensual Sales / Ventas mensuales
  * Top Month / Mejor mes
  * Top Products / Productos (3, 5, 10, 20)

* 🔐 JWT Authentication / Autenticación JWT

* 🧑‍💼 Roles:

  * Client / Cliente
  * Admin / Admin

* 📦 DTOs (Data Transfer Objects)

* 📄 Paged Results / Resultados paginados

---

## 🚀 Estado del proyecto

Proyecto **funcional**, con foco en buenas prácticas de arquitectura, escalabilidad y experiencia de usuario.

---


