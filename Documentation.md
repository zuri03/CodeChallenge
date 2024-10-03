# Documentation
## Fixes and improvements

### DirectReports Issue
The most obvious fix that became immediately apparent upon running the application is that the 'directReports' field returned null whenever it was fetched from a context that outside of the context that was used to seed that data. After quite some investigation, I fixed this issue by using eager loading with an 'including' statement in the employee repository.

### Lack of standardized error handling
All errors are handled automatically by the .NET runtime. This means that error handling is out of the control of the developer. This is fine for small apps running in development. However, if this application was planned to move to production or a test environment I would create a centralized and dedicated error handler that would properly log every error and return the appropriate response. The appropriate response would depend on the evironemt, so there is some repitive logic needed to hanlde errors which is why I would place it in a central location such as an ExceptionFilterAttribute or a dedicated error handler.

### Lack of effective logging
The logger went unsed in many classes and can be used to display the actions of the applications. The logger should be used more to help with the development process.

### Generic string used instead of GUID
The id of each employee is a generic string which means any string can be used as an Id. The GUID or any sort of unique ID class can replaced this. This will allow the developer to ensure that any id used in the application is a string in the correct format.

### Pagination
As the datatbases grow pagination will be needed to ensure that not all records are returned at once

### Seperate end point for DirectReports
As the number of directReports for an employee grows then the size of the object returned by the getEmployeeById endpoint will grow. If the user does not need the directReports of an employee then the application will send a large amount of wasted data to the user. A seperate endpoint to get direct reports by employee id can hanlde this reponsiblity. Pagination can also be added to this endpoint to ensure that the list of direct reports returned in a single request does not grow too large.

## Design Justification
### Reporting Structure Task
For the first task I simple created a new controller method along with a new method for the service. The controller method will handle all http related concerns while the service will implement the simple business logic of generating the reporting structure. This task did not need any other extensive changes since the reporting structure was explicitly stated to not be persisted.

### Compensation Task
This task required more work. Due to the vagueness of the instructions I had to make assumptions to come up with a design that not only satisfied the explicit requirements but covered any implicit or future requirements. This is why I essentially decided to create an explicit seperation between the compensation handling and employee handling. This seperation will allow for extensive changes to the compensation handling logic without affecting the employee hanlding logic. This is why I created a dedicated controller, service, repo and db context for the compensation logic. I decided to treat the compensation db as a record of current compensation as well as compensation history. This is why the GET endpoint that needed to be created returns an array of compensation. Using this endpoint will not only fetch the compensaiton the user requires but all compensation of the employee if they deem necessary.