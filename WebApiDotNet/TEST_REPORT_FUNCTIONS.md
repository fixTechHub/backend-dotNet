# TEST REPORT - FUNCTION TESTING
## ARMS Project - Online Home Appliance Repair & Maintenance Service Management System

---

### Function 44: Review denied warranty

| Feature | Warranty Management |
|---------|-------------------|
| Test requirement | Test the functionality to review and deny warranty requests by admin |
| Number of TCs | 4 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 4 | 0 |
| Round 2 | 0 | 0 | 4 | 0 |
| Round 3 | 0 | 0 | 4 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-WARR-001 | Test viewing warranty list | 1. Login as admin<br>2. Navigate to Warranty Management<br>3. Click "View All Warranties" | Warranty list displays with status, user info, and review options | Admin logged in | Pending | | | Pending | | | Pending | | | |
| TC-WARR-002 | Test viewing warranty details | 1. Click on specific warranty item<br>2. View warranty details form | Warranty details show: user info, service details, issue description, status | Warranty exists in system | Pending | | | Pending | | | Pending | | | |
| TC-WARR-003 | Test denying warranty request | 1. Select warranty to review<br>2. Click "Deny" button<br>3. Enter denial reason<br>4. Submit | Warranty status changes to "DENIED", user notified, reason recorded | Warranty in "PENDING" status | Pending | | | Pending | | | Pending | | | |
| TC-WARR-004 | Test updating warranty status | 1. Select warranty<br>2. Change status to "DENIED"<br>3. Add resolution note | Status updated, admin review recorded, timestamp saved | Admin has permission | Pending | | | Pending | | | Pending | | | |

---

### Function 45: Lock/unlock user

| Feature | User Management |
|---------|----------------|
| Test requirement | Test the functionality to lock and unlock user accounts by admin |
| Number of TCs | 6 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 6 | 0 |
| Round 2 | 0 | 0 | 6 | 0 |
| Round 3 | 0 | 0 | 6 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-USER-001 | Test viewing user list | 1. Login as admin<br>2. Navigate to User Management<br>3. Click "View All Users" | User list displays with status, lock/unlock options | Admin logged in | Pending | | | Pending | | | Pending | | | |
| TC-USER-002 | Test locking user account | 1. Select user to lock<br>2. Click "Lock User"<br>3. Enter lock reason<br>4. Submit | User status changes to "LOCKED", reason recorded, user cannot login | User exists and is active | Pending | | | Pending | | | Pending | | | |
| TC-USER-003 | Test unlocking user account | 1. Select locked user<br>2. Click "Unlock User"<br>3. Confirm action | User status changes to "ACTIVE", user can login again | User is currently locked | Pending | | | Pending | | | Pending | | | |
| TC-USER-004 | Test lock reason validation | 1. Try to lock user without reason<br>2. Submit form | Error message displayed, lock action prevented | User selected for locking | Pending | | | Pending | | | Pending | | | |
| TC-USER-005 | Test lock history tracking | 1. Lock/unlock user multiple times<br>2. Check lock history | Lock/unlock history recorded with timestamps and reasons | User has been locked before | Pending | | | Pending | | | Pending | | | |
| TC-USER-006 | Test user login after lock | 1. Lock user account<br>2. Try to login with locked user<br>3. Check error message | Locked user cannot login, appropriate error message shown | User is locked | Pending | | | Pending | | | Pending | | | |

---

### Function 46: Filter action logs by user

| Feature | Action Logging |
|---------|----------------|
| Test requirement | Test the functionality to filter action logs by specific user |
| Number of TCs | 4 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 4 | 0 |
| Round 2 | 0 | 0 | 4 | 0 |
| Round 3 | 0 | 0 | 4 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-LOG-001 | Test viewing action logs | 1. Login as admin<br>2. Navigate to Action Logs<br>3. View log list | Action logs display with user info, actions, timestamps | Admin logged in, logs exist | Pending | | | Pending | | | Pending | | | |
| TC-LOG-002 | Test filtering by user ID | 1. Enter user ID in filter<br>2. Click "Filter"<br>3. View results | Only logs for specified user displayed | User has action logs | Pending | | | Pending | | | Pending | | | |
| TC-LOG-003 | Test filtering by date range | 1. Select date range<br>2. Apply filter<br>3. View results | Only logs within date range displayed | Logs exist in date range | Pending | | | Pending | | | Pending | | | |
| TC-LOG-004 | Test clearing filters | 1. Apply filters<br>2. Click "Clear Filters"<br>3. View results | All logs displayed, filters reset | Filters are applied | Pending | | | Pending | | | Pending | | | |

---

### Function 47: Search User

| Feature | User Management |
|---------|----------------|
| Test requirement | Test the functionality to search users by various criteria |
| Number of TCs | 5 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 5 | 0 |
| Round 2 | 0 | 0 | 5 | 0 |
| Round 3 | 0 | 0 | 5 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-SEARCH-001 | Test searching by username | 1. Enter username in search box<br>2. Click search<br>3. View results | Users matching username displayed | Users exist with that username | Pending | | | Pending | | | Pending | | | |
| TC-SEARCH-002 | Test searching by email | 1. Enter email in search box<br>2. Click search<br>3. View results | Users matching email displayed | Users exist with that email | Pending | | | Pending | | | Pending | | | |
| TC-SEARCH-003 | Test searching by phone | 1. Enter phone number<br>2. Click search<br>3. View results | Users matching phone displayed | Users exist with that phone | Pending | | | Pending | | | Pending | | | |
| TC-SEARCH-004 | Test searching by role | 1. Select role from dropdown<br>2. Click search<br>3. View results | Users with selected role displayed | Users exist with that role | Pending | | | Pending | | | Pending | | | |
| TC-SEARCH-005 | Test no results found | 1. Search for non-existent user<br>2. View results | "No users found" message displayed | Search criteria doesn't match any user | Pending | | | Pending | | | Pending | | | |

---

### Function 48: Add new service

| Feature | Service Management |
|---------|-------------------|
| Test requirement | Test the functionality to add new services to the system |
| Number of TCs | 6 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 6 | 0 |
| Round 2 | 0 | 0 | 6 | 0 |
| Round 3 | 0 | 0 | 6 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-SERVICE-001 | Test accessing add service form | 1. Login as admin<br>2. Navigate to Service Management<br>3. Click "Add New Service" | Add service form displayed with all required fields | Admin logged in | Pending | | | Pending | | | Pending | | | |
| TC-SERVICE-002 | Test adding service with valid data | 1. Fill all required fields<br>2. Upload service icon<br>3. Click "Save" | Service created successfully, success message displayed | Valid service data provided | Pending | | | Pending | | | Pending | | | |
| TC-SERVICE-003 | Test validation for duplicate service name | 1. Enter existing service name<br>2. Try to save<br>3. Check error message | Error message: "Service name already exists" | Service with same name exists | Pending | | | Pending | | | Pending | | | |
| TC-SERVICE-004 | Test validation for duplicate icon | 1. Upload existing icon<br>2. Try to save<br>3. Check error message | Error message: "Icon already exists" | Service with same icon exists | Pending | | | Pending | | | Pending | | | |
| TC-SERVICE-005 | Test required field validation | 1. Leave required fields empty<br>2. Try to save<br>3. Check validation messages | Validation errors displayed for empty fields | Form opened | Pending | | | Pending | | | Pending | | | |
| TC-SERVICE-006 | Test service icon upload | 1. Select valid image file<br>2. Upload icon<br>3. Verify upload | Icon uploaded successfully, preview displayed | Valid image file selected | Pending | | | Pending | | | Pending | | | |

---

### Function 49: Update existing service

| Feature | Service Management |
|---------|-------------------|
| Test requirement | Test the functionality to update existing services |
| Number of TCs | 5 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 5 | 0 |
| Round 2 | 0 | 0 | 5 | 0 |
| Round 3 | 0 | 0 | 5 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-UPDATE-001 | Test accessing edit service form | 1. Select service to edit<br>2. Click "Edit" button | Edit form displayed with current service data | Service exists in system | Pending | | | Pending | | | Pending | | | |
| TC-UPDATE-002 | Test updating service information | 1. Modify service details<br>2. Click "Update"<br>3. Verify changes | Service updated successfully, changes reflected in list | Valid service data provided | Pending | | | Pending | | | Pending | | | |
| TC-UPDATE-003 | Test updating service icon | 1. Upload new icon<br>2. Save changes<br>3. Verify icon update | New icon uploaded, old icon replaced | Valid image file selected | Pending | | | Pending | | | Pending | | | |
| TC-UPDATE-004 | Test validation during update | 1. Enter duplicate service name<br>2. Try to update<br>3. Check error message | Error message: "Service name already exists" | Another service with same name exists | Pending | | | Pending | | | Pending | | | |
| TC-UPDATE-005 | Test canceling update | 1. Make changes to service<br>2. Click "Cancel"<br>3. Verify no changes saved | Changes discarded, original data preserved | Service edit form opened | Pending | | | Pending | | | Pending | | | |

---

### Function 50: Deactivate service

| Feature | Service Management |
|---------|-------------------|
| Test requirement | Test the functionality to deactivate services (soft delete) |
| Number of TCs | 4 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 4 | 0 |
| Round 2 | 0 | 0 | 4 | 0 |
| Round 3 | 0 | 0 | 4 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-DEACT-001 | Test deactivating service | 1. Select active service<br>2. Click "Deactivate"<br>3. Confirm action | Service status changes to "INACTIVE", removed from active list | Service is currently active | Pending | | | Pending | | | Pending | | | |
| TC-DEACT-002 | Test viewing deactivated services | 1. Navigate to "Deleted Services"<br>2. View deactivated services list | List of deactivated services displayed | Deactivated services exist | Pending | | | Pending | | | Pending | | | |
| TC-DEACT-003 | Test restoring deactivated service | 1. Select deactivated service<br>2. Click "Restore"<br>3. Confirm action | Service status changes to "ACTIVE", appears in active list | Service is deactivated | Pending | | | Pending | | | Pending | | | |
| TC-DEACT-004 | Test confirmation dialog | 1. Click "Deactivate" without confirmation<br>2. Check dialog behavior | Confirmation dialog displayed before deactivation | Service selected for deactivation | Pending | | | Pending | | | Pending | | | |

---

### Function 51: Add new category

| Feature | Category Management |
|---------|-------------------|
| Test requirement | Test the functionality to add new categories |
| Number of TCs | 5 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 5 | 0 |
| Round 2 | 0 | 0 | 5 | 0 |
| Round 3 | 0 | 0 | 5 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-CAT-001 | Test accessing add category form | 1. Login as admin<br>2. Navigate to Category Management<br>3. Click "Add New Category" | Add category form displayed | Admin logged in | Pending | | | Pending | | | Pending | | | |
| TC-CAT-002 | Test adding category with valid data | 1. Fill category name and description<br>2. Click "Save" | Category created successfully | Valid category data provided | Pending | | | Pending | | | Pending | | | |
| TC-CAT-003 | Test validation for duplicate category name | 1. Enter existing category name<br>2. Try to save | Error message: "Category name already exists" | Category with same name exists | Pending | | | Pending | | | Pending | | | |
| TC-CAT-004 | Test required field validation | 1. Leave category name empty<br>2. Try to save | Validation error displayed | Form opened | Pending | | | Pending | | | Pending | | | |
| TC-CAT-005 | Test category list update | 1. Add new category<br>2. Check category list | New category appears in list | Category created successfully | Pending | | | Pending | | | Pending | | | |

---

### Function 52: Update existing category

| Feature | Category Management |
|---------|-------------------|
| Test requirement | Test the functionality to update existing categories |
| Number of TCs | 4 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 4 | 0 |
| Round 2 | 0 | 0 | 4 | 0 |
| Round 3 | 0 | 0 | 4 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-CAT-UPDATE-001 | Test accessing edit category form | 1. Select category to edit<br>2. Click "Edit" button | Edit form displayed with current category data | Category exists | Pending | | | Pending | | | Pending | | | |
| TC-CAT-UPDATE-002 | Test updating category information | 1. Modify category details<br>2. Click "Update" | Category updated successfully | Valid category data provided | Pending | | | Pending | | | Pending | | | |
| TC-CAT-UPDATE-003 | Test validation during update | 1. Enter duplicate category name<br>2. Try to update | Error message: "Category name already exists" | Another category with same name exists | Pending | | | Pending | | | Pending | | | |
| TC-CAT-UPDATE-004 | Test canceling update | 1. Make changes to category<br>2. Click "Cancel" | Changes discarded, original data preserved | Category edit form opened | Pending | | | Pending | | | Pending | | | |

---

### Function 53: Deactivate category

| Feature | Category Management |
|---------|-------------------|
| Test requirement | Test the functionality to deactivate categories (soft delete) |
| Number of TCs | 4 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 4 | 0 |
| Round 2 | 0 | 0 | 4 | 0 |
| Round 3 | 0 | 0 | 4 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-CAT-DEACT-001 | Test deactivating category | 1. Select active category<br>2. Click "Deactivate"<br>3. Confirm action | Category status changes to "INACTIVE" | Category is currently active | Pending | | | Pending | | | Pending | | | |
| TC-CAT-DEACT-002 | Test viewing deactivated categories | 1. Navigate to "Deleted Categories"<br>2. View deactivated categories | List of deactivated categories displayed | Deactivated categories exist | Pending | | | Pending | | | Pending | | | |
| TC-CAT-DEACT-003 | Test restoring deactivated category | 1. Select deactivated category<br>2. Click "Restore" | Category status changes to "ACTIVE" | Category is deactivated | Pending | | | Pending | | | Pending | | | |
| TC-CAT-DEACT-004 | Test confirmation dialog | 1. Click "Deactivate" without confirmation | Confirmation dialog displayed | Category selected for deactivation | Pending | | | Pending | | | Pending | | | |

---

### Function 54: Create coupon

| Feature | Coupon Management |
|---------|------------------|
| Test requirement | Test the functionality to create new coupons |
| Number of TCs | 6 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 6 | 0 |
| Round 2 | 0 | 0 | 6 | 0 |
| Round 3 | 0 | 0 | 6 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-COUPON-001 | Test accessing create coupon form | 1. Login as admin<br>2. Navigate to Coupon Management<br>3. Click "Create Coupon" | Create coupon form displayed | Admin logged in | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-002 | Test creating coupon with valid data | 1. Fill coupon details (code, discount, expiry)<br>2. Click "Create" | Coupon created successfully | Valid coupon data provided | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-003 | Test validation for duplicate coupon code | 1. Enter existing coupon code<br>2. Try to create | Error message: "Coupon code already exists" | Coupon with same code exists | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-004 | Test date validation | 1. Set expiry date in the past<br>2. Try to create | Error message: "Expiry date must be in the future" | Invalid expiry date | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-005 | Test discount amount validation | 1. Enter invalid discount amount<br>2. Try to create | Error message: "Invalid discount amount" | Invalid discount value | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-006 | Test required field validation | 1. Leave required fields empty<br>2. Try to create | Validation errors displayed | Form opened | Pending | | | Pending | | | Pending | | | |

---

### Function 55: Update coupon

| Feature | Coupon Management |
|---------|------------------|
| Test requirement | Test the functionality to update existing coupons |
| Number of TCs | 5 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 5 | 0 |
| Round 2 | 0 | 0 | 5 | 0 |
| Round 3 | 0 | 0 | 5 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-COUPON-UPDATE-001 | Test accessing edit coupon form | 1. Select coupon to edit<br>2. Click "Edit" button | Edit form displayed with current coupon data | Coupon exists | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-UPDATE-002 | Test updating coupon information | 1. Modify coupon details<br>2. Click "Update" | Coupon updated successfully | Valid coupon data provided | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-UPDATE-003 | Test validation during update | 1. Enter duplicate coupon code<br>2. Try to update | Error message: "Coupon code already exists" | Another coupon with same code exists | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-UPDATE-004 | Test updating expiry date | 1. Change expiry date<br>2. Save changes | Expiry date updated successfully | Valid new expiry date | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-UPDATE-005 | Test canceling update | 1. Make changes to coupon<br>2. Click "Cancel" | Changes discarded, original data preserved | Coupon edit form opened | Pending | | | Pending | | | Pending | | | |

---

### Function 56: Deactivate coupon

| Feature | Coupon Management |
|---------|------------------|
| Test requirement | Test the functionality to deactivate coupons (soft delete) |
| Number of TCs | 4 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 4 | 0 |
| Round 2 | 0 | 0 | 4 | 0 |
| Round 3 | 0 | 0 | 4 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-COUPON-DEACT-001 | Test deactivating coupon | 1. Select active coupon<br>2. Click "Deactivate"<br>3. Confirm action | Coupon status changes to "INACTIVE" | Coupon is currently active | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-DEACT-002 | Test viewing deactivated coupons | 1. Navigate to "Deleted Coupons"<br>2. View deactivated coupons | List of deactivated coupons displayed | Deactivated coupons exist | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-DEACT-003 | Test restoring deactivated coupon | 1. Select deactivated coupon<br>2. Click "Restore" | Coupon status changes to "ACTIVE" | Coupon is deactivated | Pending | | | Pending | | | Pending | | | |
| TC-COUPON-DEACT-004 | Test confirmation dialog | 1. Click "Deactivate" without confirmation | Confirmation dialog displayed | Coupon selected for deactivation | Pending | | | Pending | | | Pending | | | |

---

### Function 57: Add new commission configuration

| Feature | Commission Configuration |
|---------|------------------------|
| Test requirement | Test the functionality to add new commission configurations |
| Number of TCs | 5 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 5 | 0 |
| Round 2 | 0 | 0 | 5 | 0 |
| Round 3 | 0 | 0 | 5 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-COMM-001 | Test accessing create commission form | 1. Login as admin<br>2. Navigate to Commission Config<br>3. Click "Add New Configuration" | Create commission form displayed | Admin logged in | Pending | | | Pending | | | Pending | | | |
| TC-COMM-002 | Test creating commission with valid data | 1. Fill commission details (percentage, conditions)<br>2. Click "Create" | Commission configuration created successfully | Valid commission data provided | Pending | | | Pending | | | Pending | | | |
| TC-COMM-003 | Test validation for percentage range | 1. Enter percentage > 100%<br>2. Try to create | Error message: "Percentage must be between 0-100" | Invalid percentage value | Pending | | | Pending | | | Pending | | | |
| TC-COMM-004 | Test required field validation | 1. Leave required fields empty<br>2. Try to create | Validation errors displayed | Form opened | Pending | | | Pending | | | Pending | | | |
| TC-COMM-005 | Test commission list update | 1. Add new commission config<br>2. Check commission list | New configuration appears in list | Commission created successfully | Pending | | | Pending | | | Pending | | | |

---

### Function 58: Resolve booking report

| Feature | Report Management |
|---------|------------------|
| Test requirement | Test the functionality to resolve booking reports |
| Number of TCs | 4 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 4 | 0 |
| Round 2 | 0 | 0 | 4 | 0 |
| Round 3 | 0 | 0 | 4 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-REPORT-001 | Test viewing booking reports | 1. Login as admin<br>2. Navigate to Reports<br>3. View booking reports | Booking reports list displayed | Reports exist in system | Pending | | | Pending | | | Pending | | | |
| TC-REPORT-002 | Test viewing report details | 1. Click on specific report<br>2. View report details | Report details displayed with booking info and issue | Report exists | Pending | | | Pending | | | Pending | | | |
| TC-REPORT-003 | Test resolving report | 1. Select report to resolve<br>2. Add resolution note<br>3. Click "Resolve" | Report status changes to "RESOLVED" | Report is in "PENDING" status | Pending | | | Pending | | | Pending | | | |
| TC-REPORT-004 | Test resolution note validation | 1. Try to resolve without note<br>2. Check error message | Error message: "Resolution note required" | Report selected for resolution | Pending | | | Pending | | | Pending | | | |

---

### Function 59: Update system report status

| Feature | System Report Management |
|---------|-------------------------|
| Test requirement | Test the functionality to update system report status |
| Number of TCs | 4 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 4 | 0 |
| Round 2 | 0 | 0 | 4 | 0 |
| Round 3 | 0 | 0 | 4 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-SYS-REPORT-001 | Test viewing system reports | 1. Login as admin<br>2. Navigate to System Reports<br>3. View reports list | System reports list displayed | Reports exist in system | Pending | | | Pending | | | Pending | | | |
| TC-SYS-REPORT-002 | Test updating report status | 1. Select report<br>2. Change status<br>3. Add resolution note<br>4. Save | Report status updated successfully | Report exists | Pending | | | Pending | | | Pending | | | |
| TC-SYS-REPORT-003 | Test status validation | 1. Try to update with invalid status<br>2. Check error message | Error message: "Invalid status" | Report selected | Pending | | | Pending | | | Pending | | | |
| TC-SYS-REPORT-004 | Test resolution note tracking | 1. Update status with note<br>2. Check note history | Resolution note saved with timestamp | Valid status and note provided | Pending | | | Pending | | | Pending | | | |

---

### Function 60: Verify certificate

| Feature | Technician Verification |
|---------|----------------------|
| Test requirement | Test the functionality to verify technician certificates |
| Number of TCs | 4 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 4 | 0 |
| Round 2 | 0 | 0 | 4 | 0 |
| Round 3 | 0 | 0 | 4 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-CERT-001 | Test viewing certificate details | 1. Login as admin<br>2. Navigate to Technician Management<br>3. View certificate details | Certificate information displayed | Certificate exists | Pending | | | Pending | | | Pending | | | |
| TC-CERT-002 | Test verifying certificate | 1. Select certificate to verify<br>2. Click "Verify"<br>3. Add verification note | Certificate status changes to "VERIFIED" | Certificate is pending verification | Pending | | | Pending | | | Pending | | | |
| TC-CERT-003 | Test rejecting certificate | 1. Select certificate<br>2. Click "Reject"<br>3. Add rejection reason | Certificate status changes to "REJECTED" | Certificate is pending verification | Pending | | | Pending | | | Pending | | | |
| TC-CERT-004 | Test certificate validation | 1. Upload invalid certificate<br>2. Try to verify | Error message: "Invalid certificate format" | Invalid certificate file | Pending | | | Pending | | | Pending | | | |

---

### Function 61: Approve Technician

| Feature | Technician Management |
|---------|---------------------|
| Test requirement | Test the functionality to approve technician applications |
| Number of TCs | 5 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 5 | 0 |
| Round 2 | 0 | 0 | 5 | 0 |
| Round 3 | 0 | 0 | 5 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-APPROVE-001 | Test viewing technician applications | 1. Login as admin<br>2. Navigate to Technician Management<br>3. View pending applications | Pending technician applications displayed | Applications exist | Pending | | | Pending | | | Pending | | | |
| TC-APPROVE-002 | Test approving technician | 1. Select technician application<br>2. Click "Approve"<br>3. Add approval note | Technician status changes to "APPROVED" | Application is pending | Pending | | | Pending | | | Pending | | | |
| TC-APPROVE-003 | Test approval notification | 1. Approve technician<br>2. Check notification system | Technician receives approval notification | Technician approved successfully | Pending | | | Pending | | | Pending | | | |
| TC-APPROVE-004 | Test approval history tracking | 1. Approve multiple technicians<br>2. Check approval history | Approval history recorded with timestamps | Approvals performed | Pending | | | Pending | | | Pending | | | |
| TC-APPROVE-005 | Test approval validation | 1. Try to approve without required documents<br>2. Check error message | Error message: "Required documents missing" | Incomplete application | Pending | | | Pending | | | Pending | | | |

---

### Function 62: Reject technician

| Feature | Technician Management |
|---------|---------------------|
| Test requirement | Test the functionality to reject technician applications |
| Number of TCs | 4 |
| Testing Round | Passed | Failed | Pending | N/A |
| Round 1 | 0 | 0 | 4 | 0 |
| Round 2 | 0 | 0 | 4 | 0 |
| Round 3 | 0 | 0 | 4 | 0 |

| Test Case ID | Test Case Description | Test Case Procedure | Expected Results | Pre-conditions | Round 1 | Test date | Tester | Round 2 | Test date | Tester | Round 3 | Test date | Tester | Note |
|--------------|---------------------|-------------------|------------------|----------------|---------|----------|--------|---------|----------|--------|---------|----------|--------|------|
| TC-REJECT-001 | Test rejecting technician | 1. Select technician application<br>2. Click "Reject"<br>3. Add rejection reason<br>4. Submit | Technician status changes to "REJECTED" | Application is pending | Pending | | | Pending | | | Pending | | | |
| TC-REJECT-002 | Test rejection notification | 1. Reject technician<br>2. Check notification system | Technician receives rejection notification | Technician rejected successfully | Pending | | | Pending | | | Pending | | | |
| TC-REJECT-003 | Test rejection reason validation | 1. Try to reject without reason<br>2. Check error message | Error message: "Rejection reason required" | Application selected for rejection | Pending | | | Pending | | | Pending | | | |
| TC-REJECT-004 | Test rejection history tracking | 1. Reject technician<br>2. Check rejection history | Rejection history recorded with reason and timestamp | Rejection performed | Pending | | | Pending | | | Pending | | | |

---

## SUMMARY

| Function | Total Test Cases | Passed | Failed | Pending | Pass Rate |
|----------|------------------|--------|--------|---------|-----------|
| Review denied warranty | 4 | 0 | 0 | 4 | 0% |
| Lock/unlock user | 6 | 0 | 0 | 6 | 0% |
| Filter action logs by user | 4 | 0 | 0 | 4 | 0% |
| Search User | 5 | 0 | 0 | 5 | 0% |
| Add new service | 6 | 0 | 0 | 6 | 0% |
| Update existing service | 5 | 0 | 0 | 5 | 0% |
| Deactivate service | 4 | 0 | 0 | 4 | 0% |
| Add new category | 5 | 0 | 0 | 5 | 0% |
| Update existing category | 4 | 0 | 0 | 4 | 0% |
| Deactivate category | 4 | 0 | 0 | 4 | 0% |
| Create coupon | 6 | 0 | 0 | 6 | 0% |
| Update coupon | 5 | 0 | 0 | 5 | 0% |
| Deactivate coupon | 4 | 0 | 0 | 4 | 0% |
| Add new commission configuration | 5 | 0 | 0 | 5 | 0% |
| Resolve booking report | 4 | 0 | 0 | 4 | 0% |
| Update system report status | 4 | 0 | 0 | 4 | 0% |
| Verify certificate | 4 | 0 | 0 | 4 | 0% |
| Approve Technician | 5 | 0 | 0 | 5 | 0% |
| Reject technician | 4 | 0 | 0 | 4 | 0% |

**Total Test Cases**: 89  
**Total Passed**: 0  
**Total Failed**: 0  
**Total Pending**: 89  
**Overall Pass Rate**: 0%

---

**Document Version**: 1.0  
**Last Updated**: [Date]  
**Next Review**: [Date + 1 month] 