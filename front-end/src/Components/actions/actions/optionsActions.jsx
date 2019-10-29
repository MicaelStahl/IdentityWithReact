import axios from "axios";

export const IS_LOADING = "IS_LOADING";
export const ERROR_MESSAGE = "ERROR_MESSAGE";

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

/**
 * Stores a active user to the store together with its token
 * @param {*} user The user to store
 * @param {user.jwtToken} user.jwtToken token value
 * @param {user.activeId} user.activeId active ID value
 */
export const SetUser = user => {
  localStorage.setItem("jwt-token", user.jwtToken);
  localStorage.setItem("active-id", user.activeId);
};

/**
 * Returns the active users ID
 */
export const GetUser = () => {
  return localStorage.getItem("active-id");
};

/**
 * Returns the Jwt-Token for backend authentication.
 */
export const GetJwtToken = () => {
  return localStorage.getItem("jwt-token");
};

/**
 * A error call providing a message for users on what went wrong.
 * @param {*} message Message that indicates what went wrong
 */
export const ErrorMessage = message => {
  return {
    type: ERROR_MESSAGE,
    message
  };
};

/**
 * Removes the active user from the localStorage.
 * Used mostly together with SignOut.
 */
export const RemoveActiveUser = () => {
  localStorage.removeItem("jwt-token");
  localStorage.removeItem("active-id");
};
