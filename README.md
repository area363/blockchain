# Blockchain

Blockchain Implementation in C#

## Start

1. Run the following command on root directory

```
dotnet run [PORT] [ACCOUNT NAME]

ex) "dotnet run 1234 Don"
```

2. Enter private password to get private key and public key

```
ex) Enter your private password: "1234"
Your private key: "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=". Please remember it for future use. We won't be able to recover it for you if you lose it.
Your public key: "Qn6hMtLLxBDcy6jqHfNTQ8i2EpYErnAK1JbjHRKdrhwMjP5yoW9ccMu3cwmqBjkWGNZQRyK7ZohY4PuGpr6jKbYi".
```

## 1. Connect Nodes

Select option #1 and enter url of the server you want to connect

### URL Format

```
ws://127.0.0.1:[PORT]

ex) "ws://127.0.0.1:1234"
```

## 2. Add transaction

Select option #2 and enter recipient public key and amount

```
ex) Enter recipient: "RvT6jmaEDouBP8pEUY9JVpmGe4truDu52i4AdzTWgffe5gkoPwXAfFFNRmw16A2HmMPd3QZ1jR5tM4jK3U75asph"
Enter amount: "3333"
```

## 3. See blockchain

Select option #3 to see the entire chain

## 4. Get balance

Select option #4 and enter account public key

```
ex) Enter account name: "RvT6jmaEDouBP8pEUY9JVpmGe4truDu52i4AdzTWgffe5gkoPwXAfFFNRmw16A2HmMPd3QZ1jR5tM4jK3U75asph"
```

## 3. End program

Select option #5 to exit the program
