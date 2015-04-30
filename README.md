# Scrypt
**scrypt** is a basic encoding / decoding utility namely written for use encoding
and decoding [Base64](https://en.wikipedia.org/wiki/Base64) strings like the following:

```
VGhpcyBpcyBhIEJhc2U2NCBzdHJpbmcgYnVpbHQgdXNpbmcgdGhpcyB0b29sLg==
```

# Examples
To get [h]elp use:
```
script /help
```

To [e]ncode and [d]ecode use:
```
scrypt /e "Hello World!"
scrypt /d SGVsbG8gV29ybGQh
```

To [h]ash use (default is key sha1):
```
scrypt /e "Hello World!" /h
```

To [t]wist the text around use:
```
scrypt /e "Hello World!" /t
```
To [f]lip the output use:
```
scrypt /e "Hello World!" /f
```

To de[c]ipher a string use (default key is Z:W):
```
script /c "Drterc cryc" /k Z:W
```
