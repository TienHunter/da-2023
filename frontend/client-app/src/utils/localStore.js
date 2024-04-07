const localStore = {
   getItem(key) {
      try {
         const serializedValue = window.localStorage.getItem(key);
         return serializedValue ? JSON.parse(serializedValue) : null;
      } catch (error) {
         console.error("Error getting item from localStorage:", error);
         return null;
      }
   },
   setItem(key, value) {
      try {
         const serializedValue = JSON.stringify(value);
         window.localStorage.setItem(key, serializedValue);
      } catch (error) {
         console.error("Error setting item in localStorage:", error);
      }
   },
   removeItem(key) {
      try {
         window.localStorage.removeItem(key);
      } catch (error) {
         console.error("Error removing item in localStorage:", error);
      }
   },
   clear() {
      try {
         window.localStorage.clear();
      } catch (error) {
         console.error("Error clearing item in localStorage:", error);
      }
   },
};

export default localStore;
