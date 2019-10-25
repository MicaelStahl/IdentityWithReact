import React, { Component } from "react";
import { Link } from "react-router-dom";
import { connect } from "react-redux";

import Loading from "../../UI/Loading";
import Title from "../../UI/Title";
import GoBackButton from "../../UI/GoBackButton";

import * as options from "../../actions/actions/personActions";

/**
 * The details class for the Person model.
 */
class Details extends Component {
  state = {
    showMessage: false
  };

  componentDidMount() {
    this.setState({
      showMessage:
        this.props.showMessage === undefined ? false : this.props.showMessage
    });

    // this.state.showMessage === true
    //   ? setTimeout(() => this.setState({ showMessage: false }), 3000)
    //   : null;
  }

  render() {
    if (this.props.isLoading) {
      return <Loading />;
    } else {
      const person = this.props.person;
      return (
        <React.Fragment>
          <Title Title={`Details of ${person.firstName} ${person.lastName}`} />
          <GoBackButton />
          {this.state.showMessage ? <div>{this.props.message}</div> : null}
          <div>
            <div className="col-3 shadow float-left">
              <h3>Details</h3>
              <hr />
              <div className="form-group">
                <label>
                  <b>
                    <ins>Full name</ins>
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
                <p className="overflow-auto">{person.email}</p>
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
                    <ins>Postal code</ins>
                  </b>
                </label>
                <p>{person.postalCode}</p>
              </div>
            </div>
            <div className="p-2 shadow float-left ml-3">
              <h5 className="text-center">
                <ins>Options</ins>
              </h5>
              <div className="container">
                <ul className="list-unstyled">
                  <li>
                    <Link
                      onClick={() => this.props.onLinkClick(person)}
                      to={"/person/edit/:" + person.id}>
                      Edit
                    </Link>
                  </li>
                  <li>
                    <Link
                      onClick={() => this.props.onLinkClick(person)}
                      to={"/person/delete/:" + person.id}>
                      Delete
                    </Link>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </React.Fragment>
      );
    }
  }
}

const mapStateToProps = state => {
  return {
    person: state.person.person,
    isLoading: state.options.isLoading
  };
};

const mapDispatchToProps = dispatch => {
  return {
    onLinkClick: person => dispatch(options.FindPerson(person))
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Details);
