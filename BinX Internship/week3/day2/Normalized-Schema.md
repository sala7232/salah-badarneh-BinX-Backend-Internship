# Library Database - Normalized Schema

## 1. Domain and Entities

The database represents the library catalog designed on Day 1. The API needs two core entities: authors and books.

### Authors

| Attribute | Description |
|---|---|
| `Id` | Unique identifier for an author |
| `Name` | Author's name |
| `Country` | Author's country |

### Books

| Attribute | Description |
|---|---|
| `Id` | Unique identifier for a book |
| `Title` | Book title |
| `PublishedYear` | Year in which the book was published |
| `AuthorId` | Identifier of the author who wrote the book |

## 2. Normalization

### First Normal Form (1NF)

The Authors and Books tables satisfy First Normal Form because:

- Every table has a primary key.
- Every column contains one atomic value.
- There are no repeating groups or comma-separated lists.
- Each author is stored as a separate row instead of storing multiple authors in one Books column.

### Second Normal Form (2NF)

The schema satisfies Second Normal Form because:

- It already satisfies 1NF.
- Each table uses a single-column primary key named `Id`.
- In Authors, `Name` and `Country` depend on the complete primary key, `Authors.Id`.
- In Books, `Title`, `PublishedYear`, and `AuthorId` depend on the complete primary key, `Books.Id`.
- There are no composite primary keys, so no attribute can depend on only part of a primary key.

The functional dependencies are:

```text
Authors.Id -> Name, Country
Books.Id -> Title, PublishedYear, AuthorId
```

### Third Normal Form (3NF)

The schema satisfies Third Normal Form because:

- It already satisfies 2NF.
- Author information is stored only in the Authors table.
- Books stores only `AuthorId` as a foreign key.
- `AuthorName` and `AuthorCountry` are not duplicated in Books.
- Every non-key attribute depends directly on its table's primary key, not on another non-key attribute.

Keeping author details in a separate table prevents duplicate author data and update anomalies. For example, changing an author's country requires updating only one Authors row instead of every book written by that author.

## 3. Keys and Relationship

### Primary Keys

- `Authors.Id` is the primary key of the Authors table.
- `Books.Id` is the primary key of the Books table.
- Both keys use integer identity values and uniquely identify each row.

### Foreign Key

- `Books.AuthorId` is a foreign key that references `Authors.Id`.
- The foreign key prevents a book from referencing an author that does not exist.

### Relationship

The relationship between Authors and Books is one-to-many:

- One author can have zero or many books.
- Every book has one author.

This relationship supports the nested Day 1 endpoint:

```http
GET /api/v1/authors/{authorId}/books
```

## 4. SQL Server Column Types

### Authors Table

| Column | SQL Server type | Constraints |

| `Id` | `INT` | Primary Key, Identity, Not Null |
| `Name` | `NVARCHAR(150)` | Not Null |
| `Country` | `NVARCHAR(100)` | Not Null |

### Books Table

| Column | SQL Server type | Constraints |

| `Id` | `INT` | Primary Key, Identity, Not Null |
| `Title` | `NVARCHAR(250)` | Not Null |
| `PublishedYear` | `SMALLINT` | Not Null |
| `AuthorId` | `INT` | Foreign Key, Not Null |

### Type Selection Rationale

- `INT` provides enough identifiers for this library project and is supported by SQL Server identity columns.
- `NVARCHAR` supports both Arabic and English text.
- Bounded text lengths are used instead of `NVARCHAR(MAX)` because names, countries, and book titles do not require unlimited storage.
- `SMALLINT` is sufficient for publication years and uses less storage than `INT`.
- `Books.AuthorId` and `Authors.Id` both use `INT`, ensuring compatible foreign-key and primary-key types.

### Monetary Values

The current library catalog has no monetary attributes. If a monetary attribute such as `Price` or `LateFee` is added later, it should use `DECIMAL(10,2)`, not `FLOAT`, to avoid floating-point rounding errors.

## 5. SQL Server Schema

```sql
CREATE DATABASE LibraryDb;
GO

USE LibraryDb;
GO

CREATE TABLE Authors
(
    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(150) NOT NULL,
    Country NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_Authors PRIMARY KEY (Id)
);
GO

CREATE TABLE Books
(
    Id INT IDENTITY(1,1) NOT NULL,
    Title NVARCHAR(250) NOT NULL,
    PublishedYear SMALLINT NOT NULL,
    AuthorId INT NOT NULL,

    CONSTRAINT PK_Books PRIMARY KEY (Id),
    CONSTRAINT FK_Books_Authors
        FOREIGN KEY (AuthorId)
        REFERENCES Authors(Id),
    CONSTRAINT CK_Books_PublishedYear
        CHECK (PublishedYear BETWEEN 1000 AND 9999)
);
GO
```

## 6. Tools Used

- **SQL Server 2025 Developer:** Created and stored the `LibraryDb` database, tables, keys, and constraints.
- **SQL Server Management Studio 22:** Connected to SQL Server, executed the SQL schema, and inspected the created database objects.
- **Visual Studio Code:** Wrote and maintained the schema documentation.
