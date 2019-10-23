import React from "react";
// import { connect } from "react-redux";
import { NavLink } from "react-router-dom";

const Navbar = props => {
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
              <li className="nav-item">
                <NavLink className="btn nav-link" to="/login">
                  Login
                </NavLink>
              </li>
            </ul>
          </div>
        </nav>
      </header>
    </React.Fragment>
  );
};

export default Navbar;
