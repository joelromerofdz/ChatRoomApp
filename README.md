# ChatRoomApp
Chat room application is a app where you can chat with any user that create an account. Coding challenge.

# Mandatory Features
● Allow registered users to log in and talk with other users in a chatroom.</br>
● Allow users to post messages as commands into the chatroom with the following format
/stock=stock_code</br>
● Create a decoupled bot that will call an API using the stock_code as a parameter
(https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the
stock_code)</br>
● The bot should parse the received CSV file and then it should send a message back into
the chatroom using a message broker like RabbitMQ. The message will be a stock quote
using the following format: “APPL.US quote is $93.42 per share”. The post owner will be
the bot.</br>
● Have the chat messages ordered by their timestamps and show only the last 50
messages.</br>
● Unit test the functionality you prefer.

# Bonus (Optional)
● Have more than one chatroom.</br>
● Use .NET identity for users authentication</br>
● Handle messages that are not understood or any exceptions raised within the bot.</br>

## BackEnd Side technologies
&bull; C# .NET 6.0 Web API</br>
&bull; C# .NET 6.0 Web</br>
&bull; Entity Framework Core</br>
&bull; Linq</br>
&bull; AspNetCore Identity 6.0</br>
&bull; Nunit Test</br>
&bull; Moq

## FrontEnd Side:
&bull; Razor</br>
&bull; CSS</br>
&bull; Bootstrap</br>
&bull; Javascript</br>
&bull; SignalR client library</br>

## Patterns:
&bull; MVC (Model-View-Controller)</br>
&bull; Dependency Injection</br>
&bull; AspNetCore Identity Client 6.0</br>

## How to used it:
# Step 1:
<img width="960" alt="Step1" src="https://github.com/joelromerofdz/ChatRoomApp/assets/28847733/a48d0012-8703-416c-8f19-4050548be348">

# Step 2:
<img width="944" alt="Step2" src="https://github.com/joelromerofdz/ChatRoomApp/assets/28847733/cc87a41d-f5f0-4ac4-b2ff-0f1032430bb0">

# Step 3:
The second tab has to be an incognito mode window in order to log in with another user.

<img width="935" alt="Step3" src="https://github.com/joelromerofdz/ChatRoomApp/assets/28847733/16feb999-e1f6-44a7-9b1a-ccc7d30ccf18">

# Step 4:
<img width="960" alt="Step4" src="https://github.com/joelromerofdz/ChatRoomApp/assets/28847733/3d91c2d8-555e-47d4-9052-2d36427c53c7">

# Step 5:
<img width="940" alt="Step5" src="https://github.com/joelromerofdz/ChatRoomApp/assets/28847733/026914cc-37a1-4af6-a258-58151858a20a">

# Step 6:
<img width="947" alt="Step6" src="https://github.com/joelromerofdz/ChatRoomApp/assets/28847733/15f1086a-e188-47ce-905c-9648106cfd64">

# Step 7:
<img width="951" alt="Step7" src="https://github.com/joelromerofdz/ChatRoomApp/assets/28847733/757084e9-dba1-407c-8444-9afaa6d7d0b8">

# Testing the bot:
# "Note:" that stock message and response are not inserted in the DataBase.
<img width="920" alt="Step8" src="https://github.com/joelromerofdz/ChatRoomApp/assets/28847733/6bc8400d-2d10-4b1a-8911-700180a6b9eb">

