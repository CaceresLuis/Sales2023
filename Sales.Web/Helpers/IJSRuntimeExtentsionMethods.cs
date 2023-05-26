using Microsoft.JSInterop;

namespace Sales.Web.Helpers
{
    public static class IJSRuntimeExtentsionMethods
    {
        public static ValueTask<object> SetLocalStorage(this IJSRuntime js, string key, string content) 
            => js.InvokeAsync<object>("localStorage.setItem", key, content);

        public static ValueTask<object> GetLocalStorage(this IJSRuntime js, string key)
            => js.InvokeAsync<object>("localStorage.getItem", key);

        public static ValueTask<object> RemoveLocalStorage(this IJSRuntime js, string key)
            => js.InvokeAsync<object>("localStorage.removeItem", key);
    }
}
