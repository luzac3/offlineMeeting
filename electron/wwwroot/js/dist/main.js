/******/ (() => { // webpackBootstrap
/******/ 	var __webpack_modules__ = ({

/***/ "./node_modules/electron/index.js":
/*!****************************************!*\
  !*** ./node_modules/electron/index.js ***!
  \****************************************/
/***/ ((module, __unused_webpack_exports, __webpack_require__) => {

var __dirname = "/";
const fs = __webpack_require__(Object(function webpackMissingModule() { var e = new Error("Cannot find module 'fs'"); e.code = 'MODULE_NOT_FOUND'; throw e; }()));
const path = __webpack_require__(Object(function webpackMissingModule() { var e = new Error("Cannot find module 'path'"); e.code = 'MODULE_NOT_FOUND'; throw e; }()));

const pathFile = path.join(__dirname, 'path.txt');

function getElectronPath () {
  let executablePath;
  if (fs.existsSync(pathFile)) {
    executablePath = fs.readFileSync(pathFile, 'utf-8');
  }
  if (process.env.ELECTRON_OVERRIDE_DIST_PATH) {
    return path.join(process.env.ELECTRON_OVERRIDE_DIST_PATH, executablePath || 'electron');
  }
  if (executablePath) {
    return path.join(__dirname, 'dist', executablePath);
  } else {
    throw new Error('Electron failed to install correctly, please delete node_modules/electron and try installing again');
  }
}

module.exports = getElectronPath();


/***/ }),

/***/ "./wwwroot/ts/main/CreateWindow.ts":
/*!*****************************************!*\
  !*** ./wwwroot/ts/main/CreateWindow.ts ***!
  \*****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
var __dirname = "/";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   CreateWindow: () => (/* binding */ CreateWindow)
/* harmony export */ });
/* harmony import */ var _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../share/CreateLog */ "./wwwroot/ts/share/CreateLog.ts");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! electron */ "./node_modules/electron/index.js");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(electron__WEBPACK_IMPORTED_MODULE_1__);
Object(function webpackMissingModule() { var e = new Error("Cannot find module 'path'"); e.code = 'MODULE_NOT_FOUND'; throw e; }());



class CreateWindow {
    constructor() {
        this.CreateWindow = (url, show = true) => {
            const win = new electron__WEBPACK_IMPORTED_MODULE_1__.BrowserWindow({
                width: 800,
                height: 600,
                show: false,
                webPreferences: {
                    preload: Object(function webpackMissingModule() { var e = new Error("Cannot find module 'path'"); e.code = 'MODULE_NOT_FOUND'; throw e; }())(__dirname, "preload.js"),
                    contextIsolation: true,
                    nodeIntegration: false
                }
            });
            // 読み込み完了で表示
            win.once('ready-to-show', () => {
                if (show)
                    win.show();
            });
            try {
                win.loadURL(url);
            }
            catch (err) {
                const errMsg = err instanceof Error ? (err.stack ?? err.message) : String(err);
                const msg = `loadURL failed url=${url} err=${errMsg}`;
                _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
            }
            // did-fail-load を監視してログ出力
            win.webContents.on('did-fail-load', (event, errorCode, errorDescription, validatedURL, isMainFrame) => {
                const msg = `did-fail-load url=${validatedURL} code=${errorCode} desc=${errorDescription} isMainFrame=${isMainFrame}`;
                _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
            });
            return win;
        };
    }
}


/***/ }),

/***/ "./wwwroot/ts/main/Printer.ts":
/*!************************************!*\
  !*** ./wwwroot/ts/main/Printer.ts ***!
  \************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   Printer: () => (/* binding */ Printer)
/* harmony export */ });
/* harmony import */ var _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../share/CreateLog */ "./wwwroot/ts/share/CreateLog.ts");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! electron */ "./node_modules/electron/index.js");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(electron__WEBPACK_IMPORTED_MODULE_1__);
var _a;


class Printer {
}
_a = Printer;
Printer.Print = async (event, args) => {
    try {
        const url = typeof args === 'string' ? args : args?.url;
        if (!url) {
            const msg = 'Printer.Print: no url provided';
            console.error(msg);
            void _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
            return;
        }
        const printWindow = new electron__WEBPACK_IMPORTED_MODULE_1__.BrowserWindow({
            show: false,
            webPreferences: {
                contextIsolation: true
            }
        });
        try {
            await printWindow.loadURL(url);
        }
        catch (loadErr) {
            const errMsg = loadErr instanceof Error ? (loadErr.stack ?? loadErr.message) : String(loadErr);
            const msg = `Printer.Print: loadURL failed url=${url} err=${errMsg}`;
            _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
        }
        printWindow.webContents.on('did-finish-load', () => {
            try {
                printWindow.webContents.print({
                    silent: true,
                    deviceName: 'Brother HL-2240D series'
                }, (success, failureReason) => {
                    if (!success) {
                        const msg = `Printer.Print: print failed url=${url} reason=${failureReason}`;
                        _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
                    }
                    try {
                        printWindow.close();
                    }
                    catch (e) { /* ignore */ }
                });
            }
            catch (printErr) {
                const errMsg = printErr instanceof Error ? (printErr.stack ?? printErr.message) : String(printErr);
                const msg = `Printer.Print: print exception url=${url}} err=${errMsg}`;
                _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
                try {
                    printWindow.close();
                }
                catch (e) { /* ignore */ }
            }
        });
    }
    catch (err) {
        const msg = `Printer.Print: unexpected error ${err && err.stack ? err.stack : String(err)}`;
        _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
    }
};


/***/ }),

/***/ "./wwwroot/ts/main/SetElectron.ts":
/*!****************************************!*\
  !*** ./wwwroot/ts/main/SetElectron.ts ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   SetElectron: () => (/* binding */ SetElectron)
/* harmony export */ });
/* harmony import */ var _CreateWindow__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./CreateWindow */ "./wwwroot/ts/main/CreateWindow.ts");
/* harmony import */ var _Printer__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./Printer */ "./wwwroot/ts/main/Printer.ts");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! electron */ "./node_modules/electron/index.js");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(electron__WEBPACK_IMPORTED_MODULE_2__);



class SetElectron {
    constructor() {
        const createWindow = new _CreateWindow__WEBPACK_IMPORTED_MODULE_0__.CreateWindow();
        createWindow.CreateWindow('https://sample');
        electron__WEBPACK_IMPORTED_MODULE_2__.ipcMain.handle('printReceipt', (event, args) => _Printer__WEBPACK_IMPORTED_MODULE_1__.Printer.Print(event, args));
    }
}


/***/ }),

/***/ "./wwwroot/ts/share/CreateLog.ts":
/*!***************************************!*\
  !*** ./wwwroot/ts/share/CreateLog.ts ***!
  \***************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   CreateLog: () => (/* binding */ CreateLog)
/* harmony export */ });
Object(function webpackMissingModule() { var e = new Error("Cannot find module 'path'"); e.code = 'MODULE_NOT_FOUND'; throw e; }());
Object(function webpackMissingModule() { var e = new Error("Cannot find module 'fs'"); e.code = 'MODULE_NOT_FOUND'; throw e; }());
var _a;


class CreateLog {
    constructor() {
        this.OutputWholeError = () => {
            // グローバル未捕捉エラーもログに落とす（main プロセス全体の安全ネット）
            process.on('uncaughtException', (err) => {
                // eslint-disable-next-line no-console
                console.error('uncaughtException', err);
                void _a.WriteLog(`uncaughtException: ${err && err.stack ? err.stack : String(err)}`);
            });
            process.on('unhandledRejection', (reason) => {
                // eslint-disable-next-line no-console
                console.error('unhandledRejection', reason);
                void _a.WriteLog(`unhandledRejection: ${String(reason)}`);
            });
        };
        this.OutputWholeError();
    }
    static pad(n) { return String(n).padStart(2, '0'); }
}
_a = CreateLog;
CreateLog.WriteLog = async (message) => {
    try {
        const dir = Object(function webpackMissingModule() { var e = new Error("Cannot find module 'path'"); e.code = 'MODULE_NOT_FOUND'; throw e; }())(process.cwd(), 'wwwroot', 'log');
        await Object(function webpackMissingModule() { var e = new Error("Cannot find module 'fs'"); e.code = 'MODULE_NOT_FOUND'; throw e; }()).mkdir(dir, { recursive: true });
        const file = _a.MakeLogFilePath();
        const timestamp = new Date().toISOString();
        const entry = `[${timestamp}] ${message}\n`;
        await Object(function webpackMissingModule() { var e = new Error("Cannot find module 'fs'"); e.code = 'MODULE_NOT_FOUND'; throw e; }()).appendFile(file, entry, 'utf8');
    }
    catch (e) {
        // ログ自体の失敗はコンソールに出す（ここで例外を投げない）
        // eslint-disable-next-line no-console
        console.error('Failed to write log', e);
    }
};
CreateLog.MakeLogFilePath = () => {
    const now = new Date();
    const y = now.getFullYear();
    const m = _a.pad(now.getMonth() + 1);
    const d = _a.pad(now.getDate());
    const hh = _a.pad(now.getHours());
    const mm = _a.pad(now.getMinutes());
    const ss = _a.pad(now.getSeconds());
    const fileName = `${y}${m}${d}-${hh}-${mm}-${ss}.log`;
    const dir = Object(function webpackMissingModule() { var e = new Error("Cannot find module 'path'"); e.code = 'MODULE_NOT_FOUND'; throw e; }())(process.cwd(), 'wwwroot', 'log');
    return Object(function webpackMissingModule() { var e = new Error("Cannot find module 'path'"); e.code = 'MODULE_NOT_FOUND'; throw e; }())(dir, fileName);
};


/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/compat get default export */
/******/ 	(() => {
/******/ 		// getDefaultExport function for compatibility with non-harmony modules
/******/ 		__webpack_require__.n = (module) => {
/******/ 			var getter = module && module.__esModule ?
/******/ 				() => (module['default']) :
/******/ 				() => (module);
/******/ 			__webpack_require__.d(getter, { a: getter });
/******/ 			return getter;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};
// This entry needs to be wrapped in an IIFE because it needs to be in strict mode.
(() => {
"use strict";
/*!**********************************!*\
  !*** ./wwwroot/ts/main/Index.ts ***!
  \**********************************/
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _SetElectron__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./SetElectron */ "./wwwroot/ts/main/SetElectron.ts");

(() => {
    const setElectron = new _SetElectron__WEBPACK_IMPORTED_MODULE_0__.SetElectron();
});

})();

/******/ })()
;
//# sourceMappingURL=main.js.map