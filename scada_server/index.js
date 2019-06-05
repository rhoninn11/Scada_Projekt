const express = require('express')
const app = express()
const port = 5000

app.use(express.static('../scada_display/build'))
app.get('/', (req, res) => res.send('Hello World!'))

app.get('/data', (req, res) => {
    console.log('data was requested');
    res.send('server data here')
})

app.listen(port, () => console.log(`Example app listening on port ${port}!`))