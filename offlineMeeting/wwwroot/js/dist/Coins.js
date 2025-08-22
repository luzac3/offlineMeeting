/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./wwwroot/ts/Coins/CreatePaypalButton.ts":
/*!************************************************!*\
  !*** ./wwwroot/ts/Coins/CreatePaypalButton.ts ***!
  \************************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   CreatePaypalButton: () => (/* binding */ CreatePaypalButton)
/* harmony export */ });
/* harmony import */ var _SendCoins__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./SendCoins */ "./wwwroot/ts/Coins/SendCoins.ts");

class CreatePaypalButton {
    constructor() {
        this.SetPaypalButton = () => {
            this.Create(120, 1, '#paypal_button_120');
            this.Create(600, 6, '#paypal_button_600');
            this.Create(1200, 13, '#paypal_button_1200');
            this.Create(3600, 40, '#paypal_button_3600');
            this.Create(6000, 68, '#paypal_button_6000');
            this.Create(12000, 140, '#paypal_button_12000');
        };
        this.Create = (value, coins, buttonContainer) => {
            paypal.Buttons({
                createOrder: (data, actions) => {
                    // 支払い金額などを指定
                    return actions.order.create({
                        purchase_units: [{
                                amount: {
                                    value: value
                                }
                            }]
                    });
                },
                onApprove: (data, actions) => {
                    return actions.order.capture().then((details) => {
                        const sendCoins = new _SendCoins__WEBPACK_IMPORTED_MODULE_0__.SendCoins();
                        sendCoins.setCoinsData(value, coins, details);
                    });
                }
            }).render(buttonContainer);
        };
    }
}


/***/ }),

/***/ "./wwwroot/ts/Coins/SendCoins.ts":
/*!***************************************!*\
  !*** ./wwwroot/ts/Coins/SendCoins.ts ***!
  \***************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

__webpack_require__.r(__webpack_exports__);
/* harmony export */ __webpack_require__.d(__webpack_exports__, {
/* harmony export */   SendCoins: () => (/* binding */ SendCoins)
/* harmony export */ });
/* harmony import */ var _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! @root/share/FetchApi */ "./wwwroot/ts/share/FetchApi.ts");

class SendCoins {
    constructor() {
        this.setCoinsData = (value, coins, details) => {
            let coinsEntity = {
                coins: coins,
                value: value
            };
            this.send(coinsEntity).then((data) => {
                console.log(details);
                console.log(data);
                // details.payer.name.given_name
                alert('支払いが完了しました');
            });
        };
        this.send = async (coinsEntity) => {
            const fetchApi = new _root_share_FetchApi__WEBPACK_IMPORTED_MODULE_0__.FetchApi();
            return await fetchApi.send(this.url, this.method, this.headers, coinsEntity, this.responseKind).then(async (data) => {
                return data;
            }).catch(e => {
                throw e;
            });
        };
        this.url = '/Coins/PaymentComplete';
        this.method = 'POST';
        this.headers = {
            "Content-Type": "application/json",
            'X-Requested-With': 'XMLHttpRequest'
        };
        this.responseKind = "text";
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
/*!***********************************!*\
  !*** ./wwwroot/ts/Coins/Index.ts ***!
  \***********************************/
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _CreatePaypalButton__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./CreatePaypalButton */ "./wwwroot/ts/Coins/CreatePaypalButton.ts");

(() => {
    const createPaypalButton = new _CreatePaypalButton__WEBPACK_IMPORTED_MODULE_0__.CreatePaypalButton();
    createPaypalButton.SetPaypalButton();
})();

})();

/******/ })()
;
//# sourceMappingURL=Coins.js.map