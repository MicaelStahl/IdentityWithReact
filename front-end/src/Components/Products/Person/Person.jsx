import React, { Component } from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";

import Loading from "../../UI/Loading";

import * as options from "../../actions/actions/personActions";
import Title from "../../UI/Title";
import GoBackButton from "../../UI/GoBackButton";

class Person extends Component {
  state = {};

  componentDidMount() {
    // ToDo
    this.props.onSiteLoad();
  }

  render() {
    if (this.props.isLoading) {
      return <Loading />;
    } else {
      return (
        <React.Fragment>
          <Title Title="List of all people" />
          <div>
            <GoBackButton />
            <Link
              className="btn btn-primary btn-sm float-right"
              to="/person/create-new-person">
              Create new person
            </Link>
          </div>
          <div>
            <table className="table">
              <caption>List of all people</caption>
              <thead>
                <tr>
                  <th>Name</th>
                  <th>Age</th>
                  <th>Email</th>
                  <th>Phonenumber</th>
                  <th>City</th>
                  <th>Postal Code</th>
                  <th>Options</th>
                </tr>
              </thead>
              <tbody>
                {this.props.people.map(person => (
                  <tr key={person.id}>
                    <td>
                      {person.firstName} {person.lastName}
                    </td>
                    <td>{person.age}</td>
                    <td>{person.email}</td>
                    <td>{person.phoneNumber}</td>
                    <td>{person.city}</td>
                    <td>{person.postalCode}</td>
                    <td>
                      <Link
                        onClick={() =>
                          this.props.onLinkClick(person.id, this.props.people)
                        }
                        to={"/person/edit/:" + person.id}>
                        <i className="fa fa-pen-square" />
                      </Link>
                      <Link
                        onClick={() =>
                          this.props.onLinkClick(person.id, this.props.people)
                        }
                        to={"/person/details/:" + person.id}>
                        <i className="fa fa-info-circle ml-1" />
                      </Link>
                      <Link
                        onClick={() =>
                          this.props.onLinkClick(person.id, this.props.people)
                        }
                        to={"/person/delete/:" + person.id}>
                        <i className="fa fa-window-close ml-1" />
                      </Link>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </React.Fragment>
      );
    }
  }
}

const mapStateToProps = state => {
  return {
    people: state.person.people,
    isLoading: state.options.isLoading
  };
};

const mapDispatchToProps = dispatch => {
  return {
    onSiteLoad: () => dispatch(options.FindAllAsync()),
    onLinkClick: (id, people) => dispatch(options.FindPersonAsync(id, people))
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Person);
