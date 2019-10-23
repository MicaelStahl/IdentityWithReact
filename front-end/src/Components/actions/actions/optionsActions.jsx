import axios from "axios";

export const IS_LOADING = "IS_LOADING";
// export const GET_USER = "GET_USER";
// export const GET_USERS = "GET_USERS";

/**
 * Method handling loading functions.
 * @param {isLoading} isLoading - Boolean value deciding if the application is loading
 */
export const IsLoading = (isLoading = false) => {
  return {
    type: IS_LOADING,
    isLoading
  };
};

/**
 * Returns a cancelToken for Async calls to back-end API.
 */
export const CancelToken = () => {
  let cancelToken = axios.CancelToken;
  const source = cancelToken.source();
  return source.token;
};
