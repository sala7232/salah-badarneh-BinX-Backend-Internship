# Library API REST Resource Map

## 1. Domain

This API represents a small library catalog. It manages books and their authors.

## 2. Core Resources

- `books`
- `authors`

Resource names are plural nouns. Actions are represented by HTTP methods rather than verbs in the URL.

## 3. API Versioning Convention

The API uses URL-based versioning:


/api/v1/{resource}


Example:

```http
GET /api/v1/books
```

## 4. Primary Resource: Books

Each book will be identified by a unique `id`. The previous API used `PublishedYear` to find a book because the model did not have an ID. The new design uses an ID because multiple books can have the same publication year.

| Operation | HTTP Method | Endpoint | Success response | Error response |
|---|---|---|---|---|
| List all books | `GET` | `/api/v1/books` | `200 OK` - returns the list; an empty list is valid | `400 Bad Request` - invalid query parameters |
| Get one book | `GET` | `/api/v1/books/{id}` | `200 OK` - returns the requested book | `404 Not Found` - no book has the supplied ID |
| Create a book | `POST` | `/api/v1/books` | `201 Created` - returns the created book and a `Location` header | `400 Bad Request` - missing or invalid book data |
| Replace a book | `PUT` | `/api/v1/books/{id}` | `200 OK` - returns the updated book | `404 Not Found` - no book has the supplied ID; `400 Bad Request` - invalid data |
| Delete a book | `DELETE` | `/api/v1/books/{id}` | `204 No Content` - book deleted successfully | `404 Not Found` - no book has the supplied ID |

## 5. Nested Resource

```http
GET /api/v1/authors/{authorId}/books
```

- Success: `200 OK` - returns all books that belong to the author. An empty list is valid.
- Error: `404 Not Found` - the author does not exist.

This nested endpoint reflects the existing relationship in the library domain: every `Book` has an `Author`.
