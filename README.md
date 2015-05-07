# Scrypt
**scrypt** is a basic encoding / decoding utility namely written for use encoding
and decoding [Base64](https://en.wikipedia.org/wiki/Base64) strings like the following:

```
VGhpcyBpcyBhIEJhc2U2NCBzdHJpbmcgYnVpbHQgdXNpbmcgdGhpcyB0b29sLg==
```

# Examples

To get [help] use:
```
script /help
```

**Basic Commands:**

To [e]ncode and [d]ecode use:
```
scrypt /e "Hello World!"
scrypt /d SGVsbG8gV29ybGQh
```

To he[x] encode and decode use:
```
script /x "Hello World!"
script /x "48656C6C6F20576F726C6421"
```

To [t]wist the character case use:
```
scrypt /t "Hello World!"
```
To [f]lip the characters around use:
```
scrypt /f "Hello World!"
```

To output an alias use:
```
script "#alphabet"
script !alphabet
```

**Advanced Commands:**

> **Note:** The following commands require a key to execute.

To [h]ash use:
```
scrypt /h "Hello World!"
```

To [c]ipher use:
```
script /c "Drterc cryc"
```

**Having Fun:**

Each command executes according to the order of operations defined in the [Options.cs](https://github.com/disruptorsgames/scrypt/blob/master/scrypt/CommandLine/Options.cs) file so you can do things like the following

```
script /d !example /x
script /d !example /c
```
