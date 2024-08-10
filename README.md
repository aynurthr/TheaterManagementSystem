# Theater Management System

## Overview

The **Theater Management System** is a web application designed to manage various aspects of a theater's operations. This includes handling seat reservations, managing show dates, processing ticket sales, and facilitating communication between users and administrators via contact posts. The application also supports an administrative panel where admins can manage the theater's operations, reply to user inquiries, and more.

## Features

### User Interface:
- **Seat Reservations**: Users can browse show dates, view available seats, and make reservations. The seat selection interface clearly indicates which seats are available, already purchased, or unavailable.
- **Contact Us**: Users can submit inquiries or messages via the contact form. Admins can respond to these messages, and users will receive the replies via email.
- **Profile Management**: Users can manage their personal information, including name, email, and phone number. They can also update their password from the profile section.
- **Forgot Password**: Users who forget their password can use the "Forgot Password" feature to receive a reset link via email. This allows them to set a new password and regain access to their account.
- **Authentication and Role-Based Access**: Users authenticate with their credentials, and different roles are assigned to users and admins. This controls access to specific functionalities based on the user's role.

### Admin Interface:
- **Show Date Management**: Admins have the ability to create, edit, and manage show dates. They can assign tickets to seats, monitor seat reservations, and update ticket prices.
- **Contact Post Management**: Admins can view, respond to, and manage user-submitted contact posts. Responses are sent directly to users via email, and the contact post is marked as answered in the system.
- **User Role and Claims Management**: Admins can manage user roles and claims, assigning different levels of access to various parts of the system. This ensures that only authorized users can perform specific administrative tasks.
- **Authentication and Security**: The platform enforces authentication for all users, with admin users having elevated privileges. Claims-based authentication is used to ensure secure access to admin features.
- **Email Notifications**: The system sends email notifications for account confirmations, password resets, and replies to user inquiries. Admins can also trigger custom email notifications as needed. 

## Technologies Used

- **Backend**: ASP.NET Core, MediatR, Entity Framework Core
- **Frontend**: Razor Pages, jQuery, Bootstrap
- **Database**: SQL Server
- **Authentication**: ASP.NET Identity
- **Email Service**: SMTP, integrated using `IEmailService`

## Installation

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or higher)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Node.js](https://nodejs.org/) (for managing frontend packages if required)

## Usage

### User Registration and Authentication

1. **Sign Up**: Users can register on the platform by filling out the signup form. They will receive a confirmation email to verify their account.
2. **Login**: Once registered, users can log in using their credentials.

### Admin Dashboard

1. **Manage Shows**: Admins can create, edit, and delete show dates, shows (or other related entities, such as actors), and news. They can also assign tickets to seats.
2. **Contact Post Management**: Admins can view messages sent by users. They can reply to these messages directly within the system, with replies being sent as emails to the users.

### Email Notifications

- Email notifications are handled using the `IEmailService` interface. To modify email settings, update the configuration in the `appsettings.json` file under the `EmailServiceOptions` section.

## Project Structure

- **Theater.Presentation**: Contains the ASP.NET Core MVC project, including views, controllers, and static files.
- **Theater.Application**: Contains the application logic, including commands, queries, and handlers.
- **Theater.Domain**: Contains the core domain models and DTOs.
- **Theater.DataAccessLayer**: Contains the database context and repositories.
- **Theater.Infrastructure**: Contains infrastructure-related services like email and file management.
