import axios from "axios";

import * as options from "./optionsActions";

export const SIGN_IN = "SIGN_IN";
export const SIGN_OUT = "SIGN_OUT";
export const REGISTER = "REGISTER";
export const GET_USER = "GET_USER";
export const GET_USERS = "GET_USERS"; // Admin only
// ToDo

const url = "http://localhost:49420/api/Account/";

//#region SignIn

const SignIn = (message, email) => {
  return {
    type: SIGN_IN,
    message,
    email
  };
};

/**
 * Function that sends a request to the backend to sign in the requested user.
 * @param {*} signIn variable containing a email and password
 */
export const SignInAsync = signIn => {
  return dispatch => {
    axios
      .post(url + "sign-in", signIn, {
        cancelToken: options.CancelToken(),
        validateStatus: function(status) {
          return status <= 500;
        },
        auth: {
          username: signIn.email,
          password: signIn.password
        }
      })
      .then(response => {
        if (response.status === 200) {
          dispatch(SignIn(response.data.message, response.data.email));

          const user = {
            activeId: response.data.activeId,
            jwtToken: response.data.jwtToken
          };

          dispatch(options.SetUser(user));
        } else if (response.status === 404 || response.status === 400) {
          dispatch(options.ErrorMessage(response.data));
        } else {
          dispatch(options.ErrorMessage("Something went wrong."));
        }
      })
      .catch(err => {
        console.error(err);

        Promise.reject(err);
      });
  };
};

//#endregion

//#region SignOut

const SignOut = message => {
  return {
    type: SIGN_OUT,
    message
  };
};

/**
 * Signs out the active user.
 */
export const SignOutAsync = () => {
  return dispatch => {
    // ToDo
    axios.interceptors.request.use(
      config => {
        const token = options.jwtToken();

        if (token !== undefined || token !== null) {
          config.headers["Authorization"] = `Bearer ${token}`;
          config.headers["Access-Control-Allow-Origin"] = "*";
          config.headers["withCredentials"] = true;
        }
        return config;
      },
      error => {
        return Promise.reject(error);
      }
    );
    axios
      .get(url + "sign-out", {
        "Content-Type": "application/json",
        cancelToken: options.CancelToken(),
        validateStatus: function(status) {
          return status <= 500;
        }
      })
      .then(response => {
        if (response.status === 200) {
          dispatch(SignOut(response.data));
        } else if (response.status === 400) {
          dispatch(options.ErrorMessage(response.data));
        } else {
          dispatch(options.ErrorMessage("Something went wrong."));
        }
      })
      .catch(err => {
        console.error(err);
        Promise.reject(err);
      });
  };
};

//#endregion

//#region

const Register = message => {
  return {
    type: REGISTER,
    message
  };
};

/**
 *
 * @param {*} user user for
 */
export const RegisterAsync = user => {
  return dispatch => {
    axios
      .post(url + "register", user, {
        cancelToken: options.CancelToken(),
        validateStatus: function(status) {
          return status <= 500;
        }
      })
      .then(response => {
        if (response.status === 200) {
          // ToDo
          dispatch(Register(response.data));
        } else if (response.status === 400) {
          dispatch(options.ErrorMessage(response.data));
        } else {
          dispatch(options.ErrorMessage("Something went wrong."));
        }
      })
      .catch(err => {
        console.error(err);
        Promise.reject(err);
      });
  };
};

//#endregion
