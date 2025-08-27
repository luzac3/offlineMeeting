/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./wwwroot/ts/Quiz/Canvas.ts":
/*!***********************************!*\
  !*** ./wwwroot/ts/Quiz/Canvas.ts ***!
  \***********************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   Canvas: () => (/* binding */ Canvas)
/* harmony export */ });
class Canvas {
    constructor() {
        this.isDrawing = false;
        this.lastX = 0;
        this.lastY = 0;
        this.SetCanvas = () => {
            this.ClearButtonElement.addEventListener("click", () => {
                this.ActivateCanvas();
            });
        };
        this.ActivateCanvas = () => {
            this.Ctx.clearRect(0, 0, this.CanvasElement.width, this.CanvasElement.height);
        };
        this.SetCanvasSizeForDpi = () => {
            const dpr = window.devicePixelRatio || 1;
            const rect = this.CanvasElement.getBoundingClientRect();
            this.CanvasElement.width = rect.width * dpr;
            this.CanvasElement.height = rect.height * dpr;
            this.CanvasElement.style.width = rect.width + "px";
            this.CanvasElement.style.height = rect.height + "px";
        };
        this.SetDrawingPc = () => {
            this.CanvasElement.addEventListener('mousedown', (e) => {
                this.isDrawing = true;
                const rect = this.CanvasElement.getBoundingClientRect();
                const dpr = window.devicePixelRatio || 1;
                this.lastX = (e.clientX - rect.left) * dpr;
                this.lastY = (e.clientY - rect.top) * dpr;
            });
            this.CanvasElement.addEventListener('mousemove', (e) => {
                if (!this.isDrawing)
                    return;
                const rect = this.CanvasElement.getBoundingClientRect();
                const dpr = window.devicePixelRatio || 1;
                const x = (e.clientX - rect.left) * dpr;
                const y = (e.clientY - rect.top) * dpr;
                if (this.lastX === x && this.lastY === y) {
                    this.Ctx.beginPath();
                    this.Ctx.arc(x, y, 1 * dpr, 0, Math.PI * 2);
                    this.Ctx.fillStyle = "#333";
                    this.Ctx.fill();
                }
                else {
                    this.Ctx.beginPath();
                    this.Ctx.moveTo(this.lastX, this.lastY);
                    this.Ctx.lineTo(x, y);
                    this.Ctx.strokeStyle = "#333";
                    this.Ctx.lineWidth = 2 * dpr;
                    this.Ctx.lineCap = "round";
                    this.Ctx.stroke();
                }
                this.lastX = x;
                this.lastY = y;
            });
            this.CanvasElement.addEventListener('mouseup', () => {
                this.isDrawing = false;
            });
            this.CanvasElement.addEventListener('mouseleave', () => {
                this.isDrawing = false;
            });
        };
        this.SetDrawingPhone = () => {
            // --- タッチイベント ---
            this.CanvasElement.addEventListener('touchstart', (e) => {
                e.preventDefault();
                this.isDrawing = true;
                const rect = this.CanvasElement.getBoundingClientRect();
                const dpr = window.devicePixelRatio || 1;
                const touch = e.touches[0];
                this.lastX = (touch.clientX - rect.left) * dpr;
                this.lastY = (touch.clientY - rect.top) * dpr;
            }, { passive: false });
            this.CanvasElement.addEventListener('touchmove', (e) => {
                if (!this.isDrawing)
                    return;
                e.preventDefault();
                const rect = this.CanvasElement.getBoundingClientRect();
                const dpr = window.devicePixelRatio || 1;
                const touch = e.touches[0];
                const x = (touch.clientX - rect.left) * dpr;
                const y = (touch.clientY - rect.top) * dpr;
                if (this.lastX === x && this.lastY === y) {
                    this.Ctx.beginPath();
                    this.Ctx.arc(x, y, 1 * dpr, 0, Math.PI * 2);
                    this.Ctx.fillStyle = "#333";
                    this.Ctx.fill();
                }
                else {
                    this.Ctx.beginPath();
                    this.Ctx.moveTo(this.lastX, this.lastY);
                    this.Ctx.lineTo(x, y);
                    this.Ctx.strokeStyle = "#333";
                    this.Ctx.lineWidth = 2 * dpr;
                    this.Ctx.lineCap = "round";
                    this.Ctx.stroke();
                }
                this.lastX = x;
                this.lastY = y;
            }, { passive: false });
            this.CanvasElement.addEventListener('touchend', () => {
                this.isDrawing = false;
            });
            this.CanvasElement.addEventListener('touchcancel', () => {
                this.isDrawing = false;
            });
        };
        this.CanvasElement = document.getElementById('textCanvas');
        this.ClearButtonElement = document.getElementById('clear');
        this.SetCanvasSizeForDpi();
        this.Ctx = this.CanvasElement.getContext('2d');
        this.SetCanvas();
        this.ActivateCanvas();
        this.SetDrawingPc();
        this.SetDrawingPhone();
        // ウィンドウリサイズ時にCanvasサイズを再設定
        window.addEventListener('resize', () => {
            this.SetCanvasSizeForDpi();
            this.ActivateCanvas();
        });
    }
}


/***/ }),

/***/ "./wwwroot/ts/Quiz/SendCanvasImage.ts":
/*!********************************************!*\
  !*** ./wwwroot/ts/Quiz/SendCanvasImage.ts ***!
  \********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   SendCanvasImage: () => (/* binding */ SendCanvasImage)
/* harmony export */ });
/* harmony import */ var _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/FetchApi */ "./wwwroot/ts/share/FetchApi.ts");

class SendCanvasImage {
    constructor() {
        this.SendCanvas = () => {
            const sendElement = document.getElementById('send');
            const canvasElement = document.getElementById('textCanvas');
            sendElement.onclick = () => {
                const dataUrl = canvasElement.toDataURL("image/png");
                this.send({ image: dataUrl }).then((data) => {
                    console.log(data);
                    alert('送信完了');
                });
            };
        };
        this.send = async (coinsEntity) => {
            const fetchApi = new _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_0__.FetchApi();
            return await fetchApi.send(this.url, this.method, this.headers, coinsEntity, this.responseKind).then(async (data) => {
                return data;
            }).catch(e => {
                throw e;
            });
        };
        this.url = '/Quiz/SendCanvasImage';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
    }
}


/***/ }),

/***/ "./wwwroot/ts/share/ControlNavbar.ts":
/*!*******************************************!*\
  !*** ./wwwroot/ts/share/ControlNavbar.ts ***!
  \*******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   ControlNavbar: () => (/* binding */ ControlNavbar)
/* harmony export */ });
class ControlNavbar {
    constructor() {
        this.setBodyPadding = () => {
            const navbar = document.querySelector('.navbar');
            if (navbar) {
                document.body.style.paddingTop = (navbar.offsetHeight + 5) + 'px';
            }
        };
        this.setBodyPadding();
        window.addEventListener('resize', this.setBodyPadding);
    }
}


/***/ }),

/***/ "./wwwroot/ts/share/FetchApi.ts":
/*!**************************************!*\
  !*** ./wwwroot/ts/share/FetchApi.ts ***!
  \**************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   FetchApi: () => (/* binding */ FetchApi)
/* harmony export */ });
class FetchApi {
    constructor() {
        this.send = async (url, method, headers, body, 
        //body: any,
        responseKind = "json", needResponseData = true) => {
            let request;
            if (method == "GET") {
                request = new Request(url, {
                    method: method,
                    headers: headers
                });
            }
            else {
                request = new Request(url, {
                    method: method,
                    headers: headers,
                    body: JSON.stringify(body)
                });
            }
            const response = await fetch(request).catch(e => { throw e; });
            if (!response.ok) {
                throw new Error(response.statusText);
            }
            if (!needResponseData) {
                return;
            }
            switch (responseKind) {
                case "json":
                    return await this.fetchResonse(response.json()).catch(e => { throw e; });
                case "text":
                    return await this.fetchResonse(response.text()).catch(e => { throw e; });
            }
        };
    }
    async fetchResonse(response) {
        return await response.then(data => {
            return data;
        })
            .catch(error => {
            // エラー処理
            console.log(error);
            throw error;
        });
    }
}


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
// This entry need to be wrapped in an IIFE because it need to be isolated against other modules in the chunk.
(() => {
/*!**********************************!*\
  !*** ./wwwroot/ts/Quiz/Index.ts ***!
  \**********************************/
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _root_share_ControlNavbar__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/ControlNavbar */ "./wwwroot/ts/share/ControlNavbar.ts");
/* harmony import */ var _SendCanvasImage__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./SendCanvasImage */ "./wwwroot/ts/Quiz/SendCanvasImage.ts");
/* harmony import */ var _Canvas__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./Canvas */ "./wwwroot/ts/Quiz/Canvas.ts");



(() => {
    new _root_share_ControlNavbar__WEBPACK_IMPORTED_MODULE_0__.ControlNavbar();
    new _Canvas__WEBPACK_IMPORTED_MODULE_2__.Canvas();
    const sendCanvasImage = new _SendCanvasImage__WEBPACK_IMPORTED_MODULE_1__.SendCanvasImage();
    sendCanvasImage.SendCanvas();
})();

})();

/******/ })()
;
//# sourceMappingURL=Quiz.js.map