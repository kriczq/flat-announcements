# flat-announcements
[![Build Status](https://dev.azure.com/Flannounce/Flannounce/_apis/build/status/arveduil.flat-announcements?branchName=master)](https://dev.azure.com/Flannounce/Flannounce/_build/latest?definitionId=1&branchName=master)

System which allows to store flat announcements.


## Configuration
1. At first run container:  
`docker run -p 22222:27017 --name mdb -d mongo:4.0.11`
2. Then start bash in container:  
`docker exec -it mdb bash`
3. Use mongo CLI  
`mongo`
4. Create FlannounceDB  
`use FlannounceDB`
