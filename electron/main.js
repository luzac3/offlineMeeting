/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./appData/ts/main/CreateWindow.ts":
/*!*****************************************!*\
  !*** ./appData/ts/main/CreateWindow.ts ***!
  \*****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   CreateWindow: () => (/* binding */ CreateWindow)
/* harmony export */ });
/* harmony import */ var _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../share/CreateLog */ "./appData/ts/share/CreateLog.ts");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! electron */ "electron");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(electron__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var path__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! path */ "path");
/* harmony import */ var path__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(path__WEBPACK_IMPORTED_MODULE_2__);



class CreateWindow {
    constructor() {
        this.CreateWindow = (url, show = true) => {
            electron__WEBPACK_IMPORTED_MODULE_1__.app.whenReady().then(async () => {
                console.log('createWindow ready');
                const win = new electron__WEBPACK_IMPORTED_MODULE_1__.BrowserWindow({
                    width: 800,
                    height: 600,
                    show: false,
                    webPreferences: {
                        preload: path__WEBPACK_IMPORTED_MODULE_2___default().join(__dirname, "preload.js"),
                        contextIsolation: true,
                        nodeIntegration: false
                    }
                });
                win.webContents.openDevTools();
                // キャッシュクリア
                await win.webContents.session.clearCache();
                win.once('ready-to-show', () => {
                    if (show)
                        win.show();
                });
                try {
                    console.log(`start loda url:${url}`);
                    win.loadURL(url);
                }
                catch (err) {
                    const errMsg = err instanceof Error ? (err.stack ?? err.message) : String(err);
                    const msg = `loadURL failed url=${url} err=${errMsg}`;
                    _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
                }
                win.webContents.on('did-fail-load', (event, errorCode, errorDescription, validatedURL, isMainFrame) => {
                    const msg = `did-fail-load url=${validatedURL} code=${errorCode} desc=${errorDescription} isMainFrame=${isMainFrame}`;
                    _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
                });
                return win;
            });
        };
    }
}


/***/ }),

/***/ "./appData/ts/main/Printer.ts":
/*!************************************!*\
  !*** ./appData/ts/main/Printer.ts ***!
  \************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   Printer: () => (/* binding */ Printer)
/* harmony export */ });
/* harmony import */ var _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../share/CreateLog */ "./appData/ts/share/CreateLog.ts");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! electron */ "electron");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(electron__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _share_GetSetting__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../share/GetSetting */ "./appData/ts/share/GetSetting.ts");



class Printer {
    constructor(resultId) {
        this.Print = async () => {
            try {
                this.printWindow.webContents.once('did-finish-load', () => {
                    this.LoadedPrint();
                });
                try {
                    await this.printWindow.loadURL(this.url);
                }
                catch (loadErr) {
                    const errMsg = loadErr instanceof Error ? (loadErr.stack ?? loadErr.message) : String(loadErr);
                    const msg = `Printer.Print: loadURL failed url=${this.url} err=${errMsg}`;
                    _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
                }
            }
            catch (err) {
                const msg = `Printer.Print: unexpected error ${err && err.stack ? err.stack : String(err)}`;
                _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
            }
        };
        this.LoadedPrint = () => {
            try {
                const printOptions = this.data.printOptions || {};
                this.printWindow.webContents.print({
                    silent: true,
                    deviceName: this.data.deviceName,
                    ...printOptions
                }, (success, failureReason) => {
                    if (!success) {
                        const msg = `Printer.Print: print failed url=${this.url} reason=${failureReason}`;
                        _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
                    }
                    try {
                        this.printWindow.close();
                    }
                    catch (e) { /* ignore */ }
                });
            }
            catch (printErr) {
                const errMsg = printErr instanceof Error ? (printErr.stack ?? printErr.message) : String(printErr);
                const msg = `Printer.Print: print exception url=${this.url} err=${errMsg}`;
                _share_CreateLog__WEBPACK_IMPORTED_MODULE_0__.CreateLog.WriteLog(msg);
                try {
                    this.printWindow.close();
                }
                catch (e) { /* ignore */ }
            }
        };
        this.data = _share_GetSetting__WEBPACK_IMPORTED_MODULE_2__.GetSetting.Setting();
        this.url = this.data.printUrlBase + resultId;
        ;
        this.printWindow = new electron__WEBPACK_IMPORTED_MODULE_1__.BrowserWindow({
            show: false,
            webPreferences: {
                contextIsolation: true
            }
        });
    }
}


/***/ }),

/***/ "./appData/ts/main/SetElectron.ts":
/*!****************************************!*\
  !*** ./appData/ts/main/SetElectron.ts ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   SetElectron: () => (/* binding */ SetElectron)
/* harmony export */ });
/* harmony import */ var _CreateWindow__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./CreateWindow */ "./appData/ts/main/CreateWindow.ts");
/* harmony import */ var _Printer__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./Printer */ "./appData/ts/main/Printer.ts");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! electron */ "electron");
/* harmony import */ var electron__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(electron__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _share_GetSetting__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../share/GetSetting */ "./appData/ts/share/GetSetting.ts");




class SetElectron {
    constructor() {
        console.log('start set electron');
        const data = _share_GetSetting__WEBPACK_IMPORTED_MODULE_3__.GetSetting.Setting();
        const createWindow = new _CreateWindow__WEBPACK_IMPORTED_MODULE_0__.CreateWindow();
        createWindow.CreateWindow(data.url);
        electron__WEBPACK_IMPORTED_MODULE_2__.ipcMain.handle('printReceipt', (event, args) => {
            args.resultIds.forEach((resultId) => {
                const printer = new _Printer__WEBPACK_IMPORTED_MODULE_1__.Printer(resultId);
                printer.Print();
            });
        });
    }
}


/***/ }),

/***/ "./appData/ts/share/CreateLog.ts":
/*!***************************************!*\
  !*** ./appData/ts/share/CreateLog.ts ***!
  \***************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   CreateLog: () => (/* binding */ CreateLog)
/* harmony export */ });
/* harmony import */ var path__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! path */ "path");
/* harmony import */ var path__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(path__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var fs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! fs */ "fs");
/* harmony import */ var fs__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(fs__WEBPACK_IMPORTED_MODULE_1__);
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
        const dir = path__WEBPACK_IMPORTED_MODULE_0___default().resolve(process.cwd(), 'appData', 'log');
        await fs__WEBPACK_IMPORTED_MODULE_1___default().promises.mkdir(dir, { recursive: true });
        const file = _a.MakeLogFilePath();
        const timestamp = new Date().toISOString();
        const entry = `[${timestamp}] ${message}\n`;
        await fs__WEBPACK_IMPORTED_MODULE_1___default().promises.appendFile(file, entry, 'utf8');
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
    const dir = path__WEBPACK_IMPORTED_MODULE_0___default().resolve(process.cwd(), 'wwwroot', 'log');
    return path__WEBPACK_IMPORTED_MODULE_0___default().join(dir, fileName);
};


/***/ }),

/***/ "./appData/ts/share/GetSetting.ts":
/*!****************************************!*\
  !*** ./appData/ts/share/GetSetting.ts ***!
  \****************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   GetSetting: () => (/* binding */ GetSetting)
/* harmony export */ });
/* harmony import */ var path__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! path */ "path");
/* harmony import */ var path__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(path__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var fs__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! fs */ "fs");
/* harmony import */ var fs__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(fs__WEBPACK_IMPORTED_MODULE_1__);


class GetSetting {
}
GetSetting.Setting = () => {
    const isDev =  true || 0;
    const settingPath = isDev
        ? path__WEBPACK_IMPORTED_MODULE_0___default().join(__dirname, 'appData/json/setting.json')
        : path__WEBPACK_IMPORTED_MODULE_0___default().join(path__WEBPACK_IMPORTED_MODULE_0___default().dirname(process.execPath), 'setting.json');
    const bufferData = fs__WEBPACK_IMPORTED_MODULE_1___default().readFileSync(settingPath);
    return JSON.parse(bufferData.toString());
};


/***/ }),

/***/ "electron":
/*!***************************!*\
  !*** external "electron" ***!
  \***************************/
/***/ ((module) => {

module.exports = require("electron");

/***/ }),

/***/ "fs":
/*!*********************!*\
  !*** external "fs" ***!
  \*********************/
/***/ ((module) => {

module.exports = require("fs");

/***/ }),

/***/ "path":
/*!***********************!*\
  !*** external "path" ***!
  \***********************/
/***/ ((module) => {

module.exports = require("path");

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
// This entry needs to be wrapped in an IIFE because it needs to be isolated against other modules in the chunk.
(() => {
/*!**********************************!*\
  !*** ./appData/ts/main/Index.ts ***!
  \**********************************/
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _SetElectron__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./SetElectron */ "./appData/ts/main/SetElectron.ts");

console.log('start electron');
new _SetElectron__WEBPACK_IMPORTED_MODULE_0__.SetElectron();

})();

/******/ })()
;
//# sourceMappingURL=main.js.map