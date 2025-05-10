# 📱demoProcessChecker

- 檢查特定程序是否執行中，並進行紀錄
- 可帶入執行序名稱，預設為 `TiWorker`，會檢查 `TiWorker.exe`
- 五秒檢查一次寫入 `*.log`，一小時為單位生成一個檔案

# 👾Development in CLI

```sh
mkdir demoProcessChecker
cd demoProcessChecker
git init
dotnet new gitignore

# create git remote and push

dotnet new console -n demoProcessChecker
cd demoProcessChecker
dotnet build
dotnet run

# Hello, World!

# write your own code
```

# 🏃‍➡️Use Case

```sh
# watch TiWorker.exe, 5 sec/time
demoProcessChecker

# watch other exe, e.g. vs2022
demoProcessChecker devenv

# watch with custom interval, e.g. 60 sec/time
demoProcessChecker devenv 60
```

# 🖥️Screen Shot

## Running in Windows Terminal

![](./assets/console.png)

- Always output the message, whether found or not.

## Log File

![](./assets/log.png)

- Logging when the process is detected as running.

# 🕜Seq Diagram

```mermaid
sequenceDiagram
    participant User
    participant Program
    participant ProcessObj as System.Diagnostics
    participant FileSystem as File System
    participant Console

    User->>Program: Start program with/without args
    activate Program
    
    Program->>Program: Parse targetProcessName
    Note over Program: Default to "TiWorker" if no args
    
    Program->>Console: Display target process info
    Console-->>User: Show monitoring start message
    
    loop Until Ctrl+C pressed
        Program->>Program: Generate logfile path with timestamp
        Note over Program: log_{targetProcessName}_{DateTime.Now:yyyy-MM-dd_HH}.txt
        
        Program->>ProcessObj: GetProcesses()
        activate ProcessObj
        ProcessObj-->>Program: Return all running processes
        deactivate ProcessObj
        
        loop For each process
            Program->>Program: Check if name matches targetProcessName
            alt Process found
                Program->>FileSystem: AppendAllText(logFilePath, logMessage)
                Program->>Console: WriteLine(logMessage)
                Console-->>User: Display process found message
            else Process not found
                Program->>Console: WriteLine("Not found" message)
                Console-->>User: Display process not found message
            end
        end
        
        alt Exception occurs
            Program->>FileSystem: AppendAllText(logFilePath, errorMessage)
            Program->>Console: WriteLine(errorMessage)
            Console-->>User: Display error message
        end
        
        Program->>Program: Thread.Sleep(10000)
        Note over Program: Wait 10 seconds before next check
    end
    
    deactivate Program
```