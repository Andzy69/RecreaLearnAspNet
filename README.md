# RecreaLearn ASP.NET Project

This project is a converted ASP.NET Core Razor Pages version of the uploaded Figma/React Recreational Skills Website.

## Open in Visual Studio

1. Extract this ZIP file.
2. Open Visual Studio 2022.
3. Click **Open a project or solution**.
4. Select `RecreaLearnAspNet.csproj`.
5. Wait for Visual Studio to restore the project.
6. Press **Ctrl + F5** or click **Run without debugging**.

## Requirements

- Visual Studio 2022
- ASP.NET and web development workload
- .NET 8 SDK

## Current Pages

- `/` Home
- `/Courses` Browse Courses
- `/CourseDetail/1` Course Detail
- `/Login` Login UI
- `/Dashboard` Learning dashboard UI

## Database Recommendation

For local Visual Studio development, use **SQL Server Express / LocalDB** because it is free and integrates well with Visual Studio.

For a simple offline student project, **SQLite** is also a good option because it is free, lightweight, and does not need a separate server.

For online hosting later, **Supabase PostgreSQL Free plan** can be considered, but check the latest limits before deployment.
