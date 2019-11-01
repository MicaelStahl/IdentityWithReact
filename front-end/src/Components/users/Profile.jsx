import React from "react";
import { connect } from "react-redux";

import Title from "../UI/Title";
import Loading from "../UI/Loading";
import GoBackButton from "../UI/GoBackButton";

const Profile = props => {
  if (props.isLoading) {
    return <Loading />;
  } else {
    console.log(props.user);
    return (
      <React.Fragment>
        <Title Title={`Details of ${props.user.firstName}`} />
        <GoBackButton />
        <div className="col-3 shadow p-3">
          <h3>Details</h3>
          <hr />
          <div className="form-group">
            <label>
              <ins>
                <b>Full name</b>
              </ins>
            </label>
            <p>
              {props.user.firstName} {props.user.lastName}
            </p>
          </div>
          <div className="form-group">
            <label>
              <ins>
                <b>Firstname</b>
              </ins>
            </label>
            <p>{props.user.firstName}</p>
          </div>
          <div className="form-group">
            <label>
              <ins>
                <b>Lastname</b>
              </ins>
            </label>
            <p>{props.user.lastName}</p>
          </div>
          <div className="form-group">
            <label>
              <ins>
                <b>Age</b>
              </ins>
            </label>
            <p>{props.user.age}</p>
          </div>
          <div className="form-group">
            <label>
              <ins>
                <b>Email</b>
              </ins>
            </label>
            <p>{props.user.email}</p>
          </div>
          <div className="form-group">
            <label>
              <ins>
                <b>Phonenumber</b>
              </ins>
            </label>
            <p>{props.user.phoneNumber}</p>
          </div>
          {props.roles.includes("Administrator") ? (
            <div className="form-group">
              <label>
                <ins>
                  <b>Admin</b>
                </ins>
              </label>
              <p>{props.user.isAdmin ? "Yes" : "No"}</p>
            </div>
          ) : null}
        </div>
      </React.Fragment>
    );
  }
};

const mapStateToProps = state => {
  return {
    // ToDo
    user: state.identity.user,
    isLoading: state.options.isLoading,
    roles: state.identity.roles
  };
};

const mapDispatchToProps = dispatch => {
  return {
    // TODo
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Profile);
