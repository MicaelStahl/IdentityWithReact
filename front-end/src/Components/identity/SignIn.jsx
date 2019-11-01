import React, { Component } from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";

import * as options from "../actions/actions/identityActions";

import Title from "../UI/Title";
import GoBackButton from "../UI/GoBackButton";

class SignIn extends Component {
  state = {
    email: "",
    password: "",
    submitted: false
  };

  handleChange = event => {
    const { name, value } = event.target;

    this.setState({ [name]: value });
  };

  handleSubmit = event => {
    event.preventDefault();

    const { email, password } = this.state;

    const user = {
      Email: email,
      Password: password
    };

    this.props.onSignInSubmit(user);

    this.setState({ submitted: true });

    setTimeout(() => {
      this.setState({ submitted: false });
    }, 2000);
  };

  render() {
    const { email, password, submitted } = this.state;
    return (
      <React.Fragment>
        <Title Title="Sign in" />
        <GoBackButton />

        {submitted === false ? null : (
          <p className="text-center text-success font-weight-bold">
            {this.props.message}
          </p>
        )}

        <div className="col-3 shadow container center p-3">
          <h3>Sign in</h3>
          <hr />
          <form onSubmit={this.handleSubmit}>
            <div className="form-group">
              <label>Email</label>
              <input
                type="text"
                name="email"
                value={email}
                onChange={this.handleChange}
                className="form-control"
                autoFocus
                required
              />
            </div>
            <div className="form-group">
              <label>Password</label>
              <input
                type="password"
                name="password"
                value={password}
                minLength="8"
                maxLength="20"
                onChange={this.handleChange}
                className="form-control"
                required
              />
            </div>
            <div className="form-group">
              <input
                type="submit"
                value="Submit"
                className="btn btn-primary btn-sm"
              />
              <div className="float-right">
                No account? <Link to="/register">Register</Link>
              </div>
            </div>
          </form>
        </div>
      </React.Fragment>
    );
  }
}

const mapStateToProps = state => {
  return {
    message: state.identity.message
  };
};

const mapDispatchToProps = dispatch => {
  return {
    onSignInSubmit: user => dispatch(options.SignInAsync(user))
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(SignIn);
