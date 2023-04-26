# ValidationsAPI

This project contains a **.NET 7.0 API** for validating a single file.

The API contains a controller (**ValidationsController**), which is used for dealing with an uploaded file and its contents. You can send an **HTTP POST** request with a selected file to validate it.

## Versions

``` https://localhost:44385/swagger/index.html ```

![image](https://user-images.githubusercontent.com/44019590/234715671-7b76d0be-8cd0-48ca-8ddf-0f72f32df30c.png)

## POST a file to validate

``` https://localhost:44385/api/v1/Validation ```

```javascript
  {
      "File": [... (some file with a .txt extension)]
  }
```

![image](https://user-images.githubusercontent.com/44019590/234716031-1344a84d-0b80-45bd-bb56-ff9a493ba54d.png)
