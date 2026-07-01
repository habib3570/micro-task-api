# MicroTaskAPI 🎯

A Micro-Tasking & Earning Platform 
backend REST API — connecting Workers who complete tasks to earn coins, 
Buyers who post tasks and pay for work, and Admins who manage the platform.

## 🚀 Tech Stack
- **Backend:** ASP.NET Core 8 Web API
- **Architecture:** Clean Architecture (Domain / Application / Infrastructure / API)
- **Database:** MS SQL Server + Entity Framework Core
- **Auth:** Firebase (client) + JWT Token (backend authorization)
- **Payments:** Stripe
- **Docs:** Swagger / OpenAPI

## 👥 User Roles
- **Worker** — Browses tasks, submits work, earns coins, withdraws money
- **Buyer** — Creates tasks, purchases coins, approves/rejects submissions
- **Admin** — Manages users, tasks, and withdrawal requests

## ✨ Core Features
- Role-based JWT authentication & authorization
- Coin-based task economy (create/update/delete with auto coin refund)
- Submission review workflow (approve/reject with payout logic)
- Stripe-powered coin purchase system
- Withdrawal system (min 200 coins, 20 coins = $1)
- Real-time notification system
- Paginated submission history
- Admin/Buyer/Worker dashboards stats API



## 🏗️ Architecture
Domain → Application → Infrastructure → API (Clean Architecture, dependency inversion)

## 🔧 Setup
1. Clone the repo
2. Update `appsettings.json` with your SQL Server connection string, JWT secret, Firebase & Stripe keys
3. Run migrations: `dotnet ef database update`
4. Run the project: `dotnet run`
5. Swagger UI available at `/swagger`
