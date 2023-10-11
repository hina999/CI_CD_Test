////const win = new BrowserWindow({
////    webPreferences: {
////        nodeIntegration: true,
////        contextIsolation: false
////    }
////})
//const path = require('path')

const win = new BrowserWindow()

win.setThumbarButtons([
    {
        tooltip: 'button1',
        icon: path.join(__dirname, '~/Content/img/icons/apple-touch-icon/favicon.png'),
        click() { console.log('button1 clicked') }
    }, {
        tooltip: 'button2',
        icon: path.join(__dirname, '~/Content/img/icons/apple-touch-icon/favicon.png'),
        flags: ['enabled', 'dismissonclick'],
        click() { console.log('button2 clicked.') }
    }
])