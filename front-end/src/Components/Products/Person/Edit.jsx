import React, { Component } from "react";
import { connect } from "react-redux";
import { Redirect } from "react-router-dom";

import Title from "../../UI/Title";
import * as options from "../../actions/actions/personActions";
import GoBackButton from "../../UI/GoBackButton";
import Loading from "../../UI/Loading";

class Edit extends Component {
  state = {
    firstName: null,
    lastName: null,
    age: null,
    email: null,
    phoneNumber: null,
    city: null,
    postalCode: null,
    editSuccess: false,
    editMessage: "",
    editFailed: false,
    redirect: false
  };

  /**
   * Capitalizes the first character in the string.
   * @param string The string to capitalize
   */
  capitalizeFirstLetter = string => {
    return string.charAt(0).toUpperCase() + string.slice(1);
  };

  /**
   * Used for validating the created person.
   * @param {person} person The created person to validate
   */
  handleValidation = person => {
    let validation = "";

    //#region FirstName

    // Validates FirstName after the following pattern: "Nils", "Nils-Erik".
    validation = person.FirstName.match(/^([\w\-\w]){2,19}$/g);

    if (validation === null || validation === undefined) {
      this.setState({
        editFailed: true,
        editMessage: `${person.FirstName} is invalid:\n
            Can only contain letters and one '-' (optional)\n
            Between 2 to 20 characters long\n
            Examples: "Nils", "Nils-Erik"`
      });
    }

    // This is to guarantee the name-standard will be held the same. (Ex: "Nils", "Nils-Erik")
    person.FirstName = person.FirstName.toLowerCase();

    if (person.FirstName.includes("-")) {
      const firstNameArray = person.FirstName.split("-");
      let firstArray = firstNameArray[0];
      let secondArray = firstNameArray[1];

      firstArray = this.capitalizeFirstLetter(firstArray);
      secondArray = this.capitalizeFirstLetter(secondArray);

      person.FirstName =
        this.capitalizeFirstLetter(firstArray) +
        "-" +
        this.capitalizeFirstLetter(secondArray);
      //   person.FirstName = firstArray + "-" + secondArray;
    } else {
      person.FirstName = this.capitalizeFirstLetter(person.FirstName);
    }

    //#endregion

    //#region LastName

    // Validates LastName after the following pattern: "Jönsson", "Samuelsson".
    validation = person.LastName.match(/^([A-Öa-ö]){2,29}$/g);

    if (validation === null || validation === undefined) {
      this.setState({
        editFailed: true,
        editMessage: `${person.LastName} is invalid:\n
        Can only contain letters\n
        Between 2 to 30 characters long\n
        Examples: "Jönsson", "Samuelsson"`
      });
    }

    person.LastName = person.LastName.toLowerCase();
    person.LastName = this.capitalizeFirstLetter(person.LastName);

    //#endregion

    //#region Age

    // Convert it to make sure it's a number.
    let age = Number(person.Age);

    if (age > 110 || age < 5) {
      return `${age} is an invalid age: \nAge must be between 5 to 110 years old.`;
    }

    person.Age = age;

    //#endregion

    // Not validating Email since that will be done on back-end.

    //#region PhoneNumber

    person.PhoneNumber = person.PhoneNumber.replace(/[\s-.,]/g, "");

    validation = person.PhoneNumber.match(/^[\d]{4,12}$/g);

    if (validation === null || validation === undefined) {
      this.setState({
        editFailed: true,
        editMessage: `${person.PhoneNumber} is invalid:\n
        Can only contain numbers\n
        Must bestow of between 4 to 12 characters.`
      });
    }

    //#endregion

    //#region City

    validation = person.City.match(/^[A-Öa-ö ]{2,60}/g);

    if (validation === null || validation === undefined) {
      this.setState({
        editFailed: true,
        editMessage: `${person.City} is invalid:\nFormat must be as follows:\n
        Exist of only letters\n
        With a length between 2 to 60 characters\n
        Examples: "Test", "Test City"`
      });
    }

    person.City = person.City.toUpperCase();

    //#endregion

    //#region PostalCode

    validation = person.PostalCode.match(/^([\d]{1,5}[-]{0,1}[\d]{0,5})$/g);

    if (validation === null || validation === undefined) {
      this.setState({
        editFailed: true,
        editMessage: `${person.PostalCode} is invalid:\n
        Format must be as follows:\n
        Only numbers and -\n
        Examples: XXXXX & XXXXX-XXXXX`
      });
    }

    //#endregion
  };

  handleChange = event => {
    const { name, value } = event.target;

    this.setState({ [name]: value });
  };

  handleSubmit = event => {
    event.preventDefault();

    const target = event.target;

    const Person = {
      Id: target.id.value,
      FirstName: target.firstName.value,
      LastName: target.lastName.value,
      Age: target.age.value,
      Email: target.email.value,
      PhoneNumber: target.phoneNumber.value,
      City: target.city.value,
      PostalCode: target.postalCode.value
    };

    this.handleValidation(Person);

    if (!this.state.editFailed) {
      this.props.onEditSubmit(Person);

      this.setState({
        editSuccess: true,
        editMessage: `${Person.FirstName} was successfully updated.`
      });

      setTimeout(() => {
        this.setState({ redirect: true });
      }, 2000);
    }
  };

  render() {
    if (this.state.redirect) {
      return <Redirect to={"/person/details/id" + this.props.person.id} />;
    }
    if (this.props.isLoading) {
      return <Loading />;
    } else {
      const {
        firstName,
        lastName,
        age,
        email,
        phoneNumber,
        city,
        postalCode,
        editFailed,
        editMessage,
        editSuccess
      } = this.state;

      const person = this.props.person;

      return (
        <React.Fragment>
          <Title
            Title={`Editing ${this.props.person.firstName}
            ${this.props.person.lastName}`}
          />
          <GoBackButton />

          {editSuccess ? (
            <p className="container text-center text-success font-weight-bold">
              {editMessage}
            </p>
          ) : null}

          {editFailed ? (
            <p className="container text-center text-danger font-weight-bold">
              {editMessage}
            </p>
          ) : null}

          <div className="col-3 shadow p-2">
            <h3>Create</h3>

            <form onSubmit={this.handleSubmit}>
              <input type="hidden" value={person.id} name="id" />
              <div className="form-group">
                <label>Firstname</label>
                <input
                  name="firstName"
                  type="text"
                  value={firstName === null ? person.firstName : firstName}
                  onChange={this.handleChange}
                  className="form-control"
                  required
                  autoFocus
                />
              </div>
              <div className="form-group">
                <label>Lastname</label>
                <input
                  name="lastName"
                  type="text"
                  value={lastName === null ? person.lastName : lastName}
                  onChange={this.handleChange}
                  className="form-control"
                  required
                />
              </div>
              <div className="form-group">
                <label>Age</label>
                <input
                  name="age"
                  type="number"
                  value={age === null ? person.age : age}
                  onChange={this.handleChange}
                  className="form-control"
                  required
                />
              </div>
              <div className="form-group">
                <label>Email</label>
                <input
                  name="email"
                  type="text"
                  value={email === null ? person.email : email}
                  onChange={this.handleChange}
                  className="form-control"
                  required
                />
              </div>
              <div className="form-group">
                <label>Phonenumber</label>
                <input
                  name="phoneNumber"
                  type="text"
                  value={
                    phoneNumber === null ? person.phoneNumber : phoneNumber
                  }
                  onChange={this.handleChange}
                  className="form-control"
                  required
                />
              </div>
              <div className="form-group">
                <label>City</label>
                <input
                  name="city"
                  type="text"
                  value={city === null ? person.city : city}
                  onChange={this.handleChange}
                  className="form-control"
                  required
                />
              </div>
              <div className="form-group">
                <label>Postal code</label>
                <input
                  name="postalCode"
                  type="text"
                  value={postalCode === null ? person.postalCode : postalCode}
                  onChange={this.handleChange}
                  className="form-control"
                  required
                />
              </div>

              <input
                type="submit"
                value="Submit"
                className="btn btn-primary btn-sm"
              />
            </form>
          </div>
        </React.Fragment>
      );
    }
  }
}

// expand on this more in the future.
const mapStateToProps = state => {
  return {
    person: state.person.person,
    isLoading: state.options.person
  };
};

const mapDispatchToProps = dispatch => {
  return {
    onEditSubmit: person => dispatch(options.EditPersonAsync(person))
  };
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Edit);
