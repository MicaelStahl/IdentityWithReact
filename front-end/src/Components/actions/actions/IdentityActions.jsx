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

const SignIn = (message, email, roles = []) => {
  return {
    type: SIGN_IN,
    message,
    email,
    roles
  };
};

/**
 * Function that sends a request to the backend to sign in the requested user.
 * @param {*} signIn variable containing a email and password
 */
export const SignInAsync = signIn => {
  return dispatch => {
    axios
      .post(url + "signin", signIn, {
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
        console.log("response", response);
        if (response.status === 200) {
          dispatch(
            SignIn(
              response.data.message,
              response.data.email,
              response.data.roles
            )
          );

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
        const token = options.GetJwtToken();

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
    const id = options.GetUser();
    axios
      .get(url + "sign-out/" + id, {
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

//#region Register

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

//#region FindUser

const FindUser = user => {
  return {
    type: GET_USER,
    user
  };
};

export const FindUserAsync = id => {
  return dispatch => {
    axios.interceptors.request.use(
      config => {
        const token = options.GetJwtToken();

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

    dispatch(options.IsLoading(true));

    const getUser = {
      ActiveId: options.GetUser(),
      UserId: id
    };

    axios
      .post(url + "find-one", getUser, {
        cancelToken: options.CancelToken(),
        validateStatus: function(status) {
          return status <= 500;
        }
      })
      .then(response => {
        if (response.status === 200) {
          const activeUser = {
            activeId: response.data.verification.activeId,
            jwtToken: response.data.verification.jwtToken
          };

          dispatch(options.SetUser(activeUser));

          const user = {
            id: response.data.id,
            userName: response.data.userName,
            firstName: response.data.firstName,
            lastName: response.data.lastName,
            age: response.data.age,
            email: response.data.email,
            phoneNumber: response.data.phoneNumber,
            isAdmin: response.data.isAdmin
          };

          dispatch(FindUser(user));
        } else if (response.status === 404 || response.status === 400) {
          dispatch(options.ErrorMessage(response.data));
        } else {
          dispatch(options.ErrorMessage("Something went wrong."));
        }
        dispatch(options.IsLoading(false));
      })
      .catch(err => {
        console.error(err);
        Promise.reject(err);
      });
  };
};

//#endregion
