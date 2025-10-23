# 🧠 String Analyzer Service — HNG Internship Stage 1 (Backend Wizards)

A RESTful API that analyzes strings and stores their computed properties.  
Built for **HNG Stage 1 (Backend Wizards)** challenge.

---

## 🚀 Overview

This service allows you to:
- Analyze strings and compute their properties (length, palindrome status, unique characters, etc.)
- Retrieve stored analyses
- Filter analyzed strings using query parameters
- Use **natural language filtering** (e.g. “all single word palindromic strings”)
- Delete existing string records

---

## 🧩 Tech Stack

- **Language:** C#  
- **Framework:** ASP.NET Core 8 / Minimal API  
- **Storage:** In-Memory (can be swapped for DB later)  
- **Hosting:** PXXL App  
- **Hashing:** SHA-256 for unique IDs  

---

## ⚙️ Setup & Installation

### 1. Clone the Repository
git clone https://github.com/Adeyemiadigun/Hng_Stage1_BackendTrack.git
cd <your-repo>
2. Restore Dependencies
bash
Copy code
dotnet restore
3. Run Locally
bash
Copy code
dotnet run
The API will start on
👉 http://localhost:5064 (or the port shown in your console).

🔑 Environment Variables
No secrets required for basic usage.
If you deploy, you can optionally define:

ini
Copy code
PORT=<your_port>
ASPNETCORE_ENVIRONMENT=Production
📚 API Endpoints
1️⃣ Create / Analyze String
POST /strings

Request

json
Copy code
{
  "value": "string to analyze"
}
Response (201 Created)

json
Copy code
{
  "id": "sha256_hash_value",
  "value": "string to analyze",
  "properties": {
    "length": 16,
    "is_palindrome": false,
    "unique_characters": 12,
    "word_count": 3,
    "sha256_hash": "abc123...",
    "character_frequency_map": {
      "s": 2,
      "t": 3
    }
  },
  "created_at": "2025-10-22T10:00:00Z"
}
Error Codes: 400, 409, 422

2️⃣ Get Specific String
GET /strings/{string_value}

Returns the computed properties for a specific analyzed string.
Error Codes: 404

3️⃣ Get All Strings (with Filters)
GET /strings?is_palindrome=true&min_length=5&max_length=20&word_count=2&contains_character=a

Supports optional filters:

is_palindrome: boolean

min_length, max_length: integer

word_count: integer

contains_character: single character

Response

json
Copy code
{
  "data": [ ... ],
  "count": 15,
  "filters_applied": { ... }
}
Error Code: 400

4️⃣ Natural-Language Filtering
GET /strings/filter-by-natural-language?query=all single word palindromic strings

Examples:

Query	Parsed Filters
"all single word palindromic strings"	word_count=1, is_palindrome=true
"strings longer than 10 characters"	min_length=11
"palindromic strings that contain the first vowel"	is_palindrome=true, contains_character=a
"strings containing the letter z"	contains_character=z

Response

json
Copy code
{
  "data": [ ... ],
  "count": 3,
  "interpreted_query": {
    "original": "all single word palindromic strings",
    "parsed_filters": {
      "word_count": 1,
      "is_palindrome": true
    }
  }
}
Error Codes: 400, 422

5️⃣ Delete String
DELETE /strings/{string_value}
Deletes the record if it exists.
Success: 204 No Content
Error: 404 Not Found

🧪 Testing the API
Use Postman, cURL, or Thunder Client to test endpoints.

Example:

bash
Copy code
curl -X POST http://localhost:5000/strings \
     -H "Content-Type: application/json" \
     -d '{"value": "madam"}'
📁 Project Structure
Copy code
/StringAnalyzer
 ├── Controllers/
 │     └── StringController.cs
 ├── DTOs/
 │     ├── StringRequestDto.cs
 │     ├── StringResponseDto.cs
 ├── Services/
 │     ├── StringAnalyzerService.cs
 │     └── NaturalLanguageParser.cs
 ├── Program.cs
 ├── README.md
 └── ...
🧠 How It Works
POST /strings

Computes length, palindrome, unique characters, word count, SHA-256 hash, and frequency map.

Stores result in memory with timestamp.

GET endpoints

Retrieve analyzed strings with or without filters.

Natural-Language filtering

Parses human phrases using regex/keywords and maps them to filter parameters.

☁️ Deployment (Railway App)
Create a new project on https://railway.com

Link your GitHub repo

Set your environment variable:

ini
Copy code
PORT=8080
Deploy and copy the generated public URL 

🧑‍💻 Author

Adeyemi Mubarak
📧 adeyemiadigun12@gmail.xom

🧩 Built with ❤️ for HNG Internship Stage 1 — Backend Wizards
