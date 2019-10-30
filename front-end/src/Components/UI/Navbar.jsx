import React from "react";
import { connect } from "react-redux";
import { NavLink } from "react-router-dom";

import * as options from "../actions/actions/identityActions";

const Navbar = props => {
  const { isAuthenticated, roles, email, onSignOutClick } = props;
  return (
    <React.Fragment>
      <header>
        <nav className="navbar navbar-expand-sm navbar-light bg-white border-bottom shadow mb-3">
          <NavLink className="nav-link nav-item" to="/">
            Start
          </NavLink>
          <button
            className="navbar-toggler"
            type="button"
            data-toggle="collapse"
            data-target=".navbar-collapse"
            aria-expanded="false"
            aria-label="Toggle navigation">
            <span className="navbar-toggler-icon" />
          </button>
          <div className="navbar-collapse justify-content-between collapse d-sm-inline-flex flex-sm-row">
            <ul className="navbar-nav mr-auto">
              <li className="nav-item">
                <NavLink className="nav-link btn" to="/person">
                  Person
                </NavLink>
              </li>
            </ul>
            <ul className="navbar-nav ml-auto">
              {isAuthenticated ? (
                <React.Fragment>
                  {roles.includes("Administrator") ? (
                    <NavLink to="/users" className="btn nav-link">
                      <i className="fa fa-users" /> Users
                    </NavLink>
                  ) : null}
                  <NavLink className="nav-link" to="/profile">
                    <small>Hello {email}!</small>
                  </NavLink>
                  <li>
                    <button
                      onClick={() => onSignOutClick()}
                      className="btn nav-link">
                      Logout
                    </button>
                  </li>
                </React.Fragment>
              ) : (
                <li className="nav-item">
                  <NavLink className="btn nav-link" to="/login">
                    Login
                  </NavLink>
                </li>
              )}
            </ul>
          </div>
        </nav>
      </header>
    </React.Fragment>
  );
};

const mapStateToProps = state => {
  return {
    isAuthenticated: state.identity.isAuthenticated,
    roles: state.identity.roles,
    email: state.identity.email
  };
};

const mapDispatchToProps = dispatch => {
  return {
    onSignOutClick: () => dispatch(options.SignOutAsync())
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Navbar);
