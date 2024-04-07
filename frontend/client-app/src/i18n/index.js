import { createI18n } from 'vue-i18n'
import viMessage from "./vi.js";
import enMessage from "./en.js";
const messages = {
   vi: viMessage,
   en: enMessage
};

const i18n = new createI18n({
   legacy: false,
   locale: 'vi',
   fallbackLocale: 'vi',
   messages,
})
// Gán biến `t` vào global scope
const { t } = i18n.global;

if (typeof window !== 'undefined') {
   window.$t = t;
}
export default i18n;