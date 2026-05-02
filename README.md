# Podcast

A Windows desktop application for managing and streaming podcast content, built with WPF and powered by AWS cloud services.

## Overview

Podcast is a .NET WPF application that integrates with Amazon Web Services to provide a cloud-backed podcast management experience. Podcast audio files are stored in Amazon S3, while user accounts and subscription data are managed through Amazon DynamoDB.

## Features

- **Podcast Management** – Upload, download, list, and delete podcast episodes stored in Amazon S3
- **User Accounts** – Create, read, update, and soft-delete user profiles via Amazon DynamoDB
- **Cloud Storage** – Scalable episode storage backed by S3 with configurable bucket support
- **Environment-based Configuration** – AWS credentials and resource names are loaded from a `.env` file, keeping secrets out of source code

## Tech Stack

| Layer | Technology |
|-------|-----------|
| UI Framework | WPF (.NET 9, Windows) |
| Language | C# |
| Cloud Storage | Amazon S3 (`AWSSDK.S3`) |
| Database | Amazon DynamoDB (`AWSSDK.DynamoDBv2`) |
| Configuration | `DotNetEnv` |

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (Windows)
- An AWS account with:
  - An S3 bucket for podcast files
  - A DynamoDB table for user data
- AWS IAM credentials with appropriate read/write permissions for both services

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/Ricardo199/Podcast.git
cd Podcast
```

### 2. Configure environment variables

Copy the provided `.env` template and fill in your values:

```bash
cp Podcast/.env.example Podcast/.env
```

Edit `Podcast/.env`:

```env
AWS_ACCESS_KEY_ID=your_access_key_id
AWS_SECRET_ACCESS_KEY_ID=your_secret_access_key
AWS_REGION=us-east-1
S3_BUCKET_NAME=your-s3-bucket-name
DYNAMODB_TABLE_NAME=your-dynamodb-table-name
```

> **Note:** The `.env` file is listed in `.gitignore` — never commit credentials to version control.

### 3. Build and run

```bash
dotnet build Podcast.sln
dotnet run --project Podcast/Podcast.csproj
```

Or open `Podcast.sln` in Visual Studio and press **F5**.

## Project Structure

```
Podcast/
├── Controller/
│   ├── S3PodcastsService.cs   # CRUD operations for podcast files (S3)
│   └── S3UsersService.cs      # CRUD operations for user profiles (DynamoDB)
├── Model/
│   ├── Podcasts.cs            # Podcast data model
│   └── User.cs                # User data model
├── MainWindow.xaml            # Main application window (WPF)
├── MainWindow.xaml.cs         # Code-behind: AWS client initialization
├── App.xaml / App.xaml.cs     # Application entry point
└── Podcast.csproj             # Project file (.NET 9, WPF)
```

## AWS Setup

### S3 Bucket

The `S3PodcastsService` expects an existing S3 bucket. It supports:

- `UploadAsync(key, stream)` – Upload a new episode
- `DownloadAsync(key)` – Stream an episode for playback
- `UpdateAsync(key, stream)` – Replace an existing episode
- `DeleteAsync(key)` – Remove an episode
- `ListAsync(prefix)` – List all episodes (optionally filtered by prefix)

### DynamoDB Table

The `S3UsersService` expects an existing DynamoDB table with `ID` as the partition key. It supports:

- `CreateUserAsync(user)` – Add a new user record
- `GetUserAsync(id)` – Retrieve a user by ID (excludes soft-deleted records)
- `UpdateUserAsync(user)` – Update user name, email, password, and podcast list
- `DeleteUserAsync(id)` – Soft-delete a user (sets `IsDeleted = true`)

## License

This project is licensed under the terms found in [LICENSE.txt](LICENSE.txt).
