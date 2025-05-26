# MTB Crux PR Interview Notes

This document contains notes about bad practices found in the PR for the MTB Crux application. Use these notes when conducting technical interviews to assess a candidate's ability to identify and address code quality issues.

## Frontend Issues

### FavoritesView.vue

1. **Direct DOM manipulation** inside template using `onclick="document.getElementById()"` - Bypasses Vue's reactivity system
2. **Missing accessibility attributes** on interactive elements
3. **No TypeScript usage** despite project consistency requirements
4. **Fragile imports** - Importing directly from file path without using aliases
5. **Global variable usage** - Using `var globalCounter = 0` in global scope
6. **Non-descriptive component name** - Using 'Favs' instead of something meaningful
7. **Side effects in mounted without cleanup** - Modifying document.body.style directly
8. **Memory leak risk** - Setting up polling with setInterval without cleanup
9. **Security vulnerability** - Direct script injection via createElement and innerHTML
10. **No cleanup on unmount** - Missing beforeUnmount hook for resource cleanup
11. **Non-scoped styles** affecting global scope - Global style definitions for common elements
12. **Using !important** and overly specific selectors - Poor CSS practice

### FavoritesList.vue

1. **Excessive inline styles** throughout the template
2. **Direct DOM access** with refs instead of using v-model
3. **Unsafe v-html usage** with unsanitized content - XSS vulnerability
4. **Global state mutation** without using store actions
5. **Hardcoded API URLs** in component methods
6. **No error handling** in API calls
7. **No loading states** for asynchronous operations
8. **No pagination** for potentially large datasets
9. **Hardcoded credentials** in frontend code - Serious security issue
10. **Memory leak risk** with global event listeners
11. **Inconsistent code style** - Not using TypeScript or setup script syntax
12. **Mixing Options API with composition** - Inconsistent component patterns
13. **Direct localStorage manipulation** in component
14. **Inefficient filtering** on every keystroke without debouncing
15. **Duplicate API fetch logic** - Violates DRY principle
16. **Timeouts without cleanup** - Potential memory leak
17. **Global event handlers** without component scope awareness
18. **Exposing sensitive data** in exports - PII and security issues
19. **Using eval()** - Serious security vulnerability
20. **Global styles** that could affect other components
21. **Overly specific CSS selectors**

### favorites.js Store

1. **Global variables** instead of proper store management
2. **Exposing API keys and tokens** in the code - Security issue
3. **Inconsistent with Pinia pattern** used elsewhere in the project
4. **Direct mutation** without immutability - Poor state management
5. **Saving sensitive data** in localStorage
6. **Manual listener notification** instead of reactive state
7. **Direct API calls** in store without proper error handling
8. **Inefficient array manipulation** methods
9. **Duplicated code** - Violates DRY principle
10. **No caching strategy** for data retrieval
11. **Synchronous code** that could block UI
12. **Custom subscription system** instead of using Vue's reactive state
13. **Inefficient notification** of listeners
14. **Mixing async and sync methods** without clear pattern
15. **Swallowing errors** without proper handling
16. **Storing sensitive user data** including tokens and API keys
17. **XSS vulnerability** with direct HTML insertion

## Backend Issues

### BadPracticeController.cs

1. **Hardcoded credentials** in the controller - Database connection string, API keys
2. **SQL Injection vulnerability** - Direct string concatenation in SQL query
3. **Leaking sensitive information** in error responses
4. **Security backdoor** implementation through special headers
5. **Exposing sensitive system information** in logs endpoint
6. **Returning unencrypted sensitive user data** - SSN, credit card numbers, passwords
7. **No input validation** or sanitization
8. **No parameterized queries** for database access
9. **No authorization** or proper access controls on most endpoints
10. **Violation of principle of least privilege** - Overly permissive data access
11. **Direct string exposure** of real user passwords without hashing

## General Issues

1. **Inconsistent coding styles** across the application
2. **Lack of comprehensive error handling**
3. **Missing logging** for security and operational events
4. **No input validation** across application
5. **Security vulnerabilities** including XSS, SQL injection, credential exposure
6. **No test coverage** for key functionality 
7. **Poorly organized code** with mixed responsibilities

Use these notes to create interview questions that test a candidate's ability to identify these issues and propose better solutions.
