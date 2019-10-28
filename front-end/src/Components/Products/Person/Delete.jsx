import React, { Component } from "react";
import { connect } from "react-redux";
import { Redirect } from "react-router-dom";

import * as options from "../../actions/actions/personActions";

import Title from "../../UI/Title";
import GoBackButton from "../../UI/GoBackButton";

class Delete extends Component {
  state = { redirect: false };

  onDelete = id => {
    this.props.onDeleteClick(id);

    setTimeout(() => {
      this.setState({ redirect: true });
    }, 2000);
  };

  render() {
    if (this.state.redirect) {
      return <Redirect to="/person" />;
    }
    const person = this.props.person;
    console.log(this.props.message);
    return (
      <React.Fragment>
        <Title Title={`Deleting ${person.firstName} ${person.lastName}`} />
        <div>
          <GoBackButton />

          <p className="float-right">
            Are you sure you want to delete {person.firstName}?{" "}
            <button
              onClick={() => this.onDelete(person.id)}
              className="btn btn-danger btn-sm">
              Delete
            </button>
          </p>
        </div>
        {this.props.message === undefined ||
        this.props.message === null ? null : (
          <p className="container text-center text-success font-weight-bold">
            {this.props.message}
          </p>
        )}

        <div className="col-3 shadow">
          <h3>Details</h3>
          <hr />

          <div className="form-group">
            <label>
              <b>
                <ins>Full-name</ins>
              </b>
            </label>
            <p>
              {person.firstName} {person.lastName}
            </p>
          </div>

          <div className="form-group">
            <label>
              <b>
                <ins>Age</ins>
              </b>
            </label>
            <p>{person.age}</p>
          </div>

          <div className="form-group">
            <label>
              <b>
                <ins>Email</ins>
              </b>
            </label>
            <p>{person.email}</p>
          </div>

          <div className="form-group">
            <label>
              <b>
                <ins>Phonenumber</ins>
              </b>
            </label>
            <p>{person.phoneNumber}</p>
          </div>

          <div className="form-group">
            <label>
              <b>
                <ins>City</ins>
              </b>
            </label>
            <p>{person.city}</p>
          </div>

          <div className="form-group">
            <label>
              <b>
                <ins>Postalcode</ins>
              </b>
            </label>
            <p>{person.postalCode}</p>
          </div>
        </div>
      </React.Fragment>
    );
  }
}

const mapStateToProps = state => {
  return {
    person: state.person.person,
    message: state.person.deleteMsg,
    isLoading: state.options.isLoading // Maybe not needed.
  };
};

const mapDispatchToProps = dispatch => {
  return {
    onDeleteClick: id => dispatch(options.DeletePersonAsync(id))
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Delete);
